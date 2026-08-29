using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class RolesFamilias : System.Web.UI.Page
{
    bllRol bllRol;
    bllFamilia bllFamilia;

    private bool ModoRol
    {
        get { return rblTipo.SelectedValue == "R"; }
    }

    // Selección "en construcción" para un perfil/familia NUEVO.
    // Guardo tokens en ViewState y reconstruyo los objetos AccesoPermiso desde la BD cuando hacen falta.
    private List<string> Seleccion 
    {
        get { return ViewState["Seleccion"] as List<string> ?? new List<string>(); }
        set { ViewState["Seleccion"] = value; }
    }

    private HashSet<string> ClavesUsuario
    {
        get { return Master.ObtenerClavesUsuario(); }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        bllRol = new bllRol();
        bllFamilia = new bllFamilia();

        if (!IsPostBack)
        {
            ConfigurarRadiosSegunPermisos();   // deja seleccionado un radio habilitado
            CargarCatalogo();
            CargarPerfiles();
            AjustarModo();          // setea combo, label y campo Clave según Perfil/Familia
            pnlAlerta.Visible = false;
        }
    }

    private void ConfigurarRadiosSegunPermisos()
    {
        HashSet<string> claves = ClavesUsuario;
        bool puedeRoles = claves.Contains("ROLES_RB");
        bool puedeFamilias = claves.Contains("FAMILIAS_RB");

        if (!puedeRoles && puedeFamilias)
            rblTipo.SelectedValue = "F";
        else if (puedeRoles)
            rblTipo.SelectedValue = "R";
        // si no tiene ninguno, queda el default; los botones se apagan
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        HashSet<string> claves = ClavesUsuario;
        bool puedeElegirTipo = claves.Contains("ROLES_RB") || claves.Contains("FAMILIAS_RB");

        if (!puedeElegirTipo)
        {
            btnCrear.Enabled = false;
            btnModificar.Enabled = false;
            btnEliminar.Enabled = false;
            btnAsignar.Enabled = false;
            btnDesasignar.Enabled = false;
        }
    }

    protected void rblTipo_SelectedIndexChanged(object sender, EventArgs e)
    {
        Seleccion = new List<string>();
        txtNombre.Text = string.Empty;
        twPermisosSeleccionados.Nodes.Clear();
        AjustarModo();
        pnlAlerta.Visible = false;
    }

    protected void ddlRolesFamilias_SelectedIndexChanged(object sender, EventArgs e)
    {
        pnlAlerta.Visible = false;

        // Volver a "-- Seleccionar --" => estoy armando uno nuevo
        if (string.IsNullOrEmpty(ddlRolesFamilias.SelectedValue))
        {
            txtNombre.Text = string.Empty;
            RenderSeleccionNueva();
            return;
        }

        int id = int.Parse(ddlRolesFamilias.SelectedValue);
        txtNombre.Text = ddlRolesFamilias.SelectedItem.Text;
        Seleccion = new List<string>(); // ya no estoy armando uno nuevo

        if (ModoRol)
        {
            RenderAccesos(bllRol.ObtenerAccesosPorRol(id));
        }
        else
        {
            Familia fam = bllFamilia.ObtenerFamiliaEspecifica(id);
            RenderAccesos(fam != null ? fam.ObtenerHijos() : new List<AccesoPermiso>());
        }
    }

    #region ABM Roles y Familias
    protected void btnCrear_Click(object sender, EventArgs e)
    {
        pnlAlerta.Visible = false;
        try
        {
            List<AccesoPermiso> accesos = Seleccion.Select(ResolverAcceso).Where(a => a != null).ToList();

            if (ModoRol)
                bllRol.CrearRol(txtNombre.Text.Trim(), accesos);
            else
                bllFamilia.CrearFamilia(txtNombre.Text.Trim(), accesos);

            LimpiarFormulario();
            RecargarTodo();
            Mostrar(ModoRol ? "Perfil creado correctamente." : "Familia creada correctamente.");
        }
        catch (Exception ex) { Mostrar(ex.Message); }
    }

    protected void btnModificar_Click(object sender, EventArgs e)
    {
        pnlAlerta.Visible = false;
        try
        {
            if (string.IsNullOrEmpty(ddlRolesFamilias.SelectedValue))
            {
                Mostrar("Seleccioná un perfil o familia del desplegable para modificar.");
                return;
            }

            int id = int.Parse(ddlRolesFamilias.SelectedValue);

            if (ModoRol)
                bllRol.ModificarRol(id, txtNombre.Text.Trim());
            else
                bllFamilia.ModificarFamilia(id, txtNombre.Text.Trim());

            RecargarTodo();
            AjustarModo();
            Mostrar(ModoRol ? "Perfil modificado correctamente." : "Familia modificada correctamente.");
        }
        catch (Exception ex) { Mostrar(ex.Message); }
    }

    protected void btnEliminar_Click(object sender, EventArgs e)
    {
        pnlAlerta.Visible = false;
        try
        {
            if (string.IsNullOrEmpty(ddlRolesFamilias.SelectedValue))
            {
                Mostrar("Seleccioná un perfil o familia del desplegable para eliminar.");
                return;
            }

            int id = int.Parse(ddlRolesFamilias.SelectedValue);

            if (ModoRol)
                bllRol.BorrarRol(id);
            else
                bllFamilia.BorrarFamilia(id);

            LimpiarFormulario();
            RecargarTodo();
            AjustarModo();
            Mostrar(ModoRol ? "Perfil eliminado correctamente." : "Familia eliminada correctamente.");
        }
        catch (Exception ex) { Mostrar(ex.Message); }
    }
    #endregion

    #region Asignaciones 
    protected void btnAsignar_Click(object sender, EventArgs e)
    {
        pnlAlerta.Visible = false;
        try
        {
            TreeNode sel = twPermisosFamilias.SelectedNode;
            if (sel == null || string.IsNullOrEmpty(sel.Value))
            {
                Mostrar("Seleccioná un permiso o familia válido del panel izquierdo.");
                return;
            }

            AccesoPermiso acceso = ResolverAcceso(sel.Value);
            if (acceso == null)
            {
                Mostrar("No se pudo resolver el permiso/familia seleccionado.");
                return;
            }

            // Caso 1: armando uno NUEVO (no hay perfil/familia elegido en el combo)
            if (string.IsNullOrEmpty(ddlRolesFamilias.SelectedValue))
            {
                List<AccesoPermiso> enCurso = Seleccion.Select(ResolverAcceso).Where(a => a != null).ToList();
                
                if (bllFamilia.VerificarPermisoFamiliaRepetida(enCurso, acceso))
                {
                    Mostrar("Ese permiso o familia ya está en la selección.");
                    return;
                }

                List<string> lista = Seleccion;
                lista.Add(sel.Value);
                Seleccion = lista;
                RenderSeleccionNueva();
                return;
            }

            // Caso 2: asignar sobre un perfil/familia EXISTENTE
            int id = int.Parse(ddlRolesFamilias.SelectedValue);
            if (ModoRol)
            {
                bllRol.AsignarAccesoAlRol(id, acceso);
                RenderAccesos(bllRol.ObtenerAccesosPorRol(id));
            }
            else
            {
                bllFamilia.AsignarAccesoAFamilia(id, acceso);
                Familia fam = bllFamilia.ObtenerFamiliaEspecifica(id);
                RenderAccesos(fam.ObtenerHijos());
            }

            CargarCatalogo();
            CargarPerfiles();
            Mostrar("Permiso/familia asignado correctamente.");
        }
        catch (Exception ex) { Mostrar(ex.Message); }
    }

    protected void btnDesasignar_Click(object sender, EventArgs e)
    {
        pnlAlerta.Visible = false;
        try
        {
            TreeNode sel = twPermisosSeleccionados.SelectedNode;
            if (sel == null || string.IsNullOrEmpty(sel.Value))
            {
                Mostrar("Seleccioná un permiso o familia del panel derecho.");
                return;
            }

            // Solo se pueden quitar accesos de primer nivel; los heredados de una
            // familia (nodos hijos) no se pueden quitar individualmente.
            if (sel.Depth > 0)
            {
                Mostrar("No se puede quitar un permiso heredado de una familia. Quitá la familia completa.");
                return;
            }

            AccesoPermiso acceso = ResolverAcceso(sel.Value);
            if (acceso == null)
            {
                Mostrar("No se pudo resolver el permiso/familia seleccionado.");
                return;
            }

            // Caso 1: armando uno NUEVO
            if (string.IsNullOrEmpty(ddlRolesFamilias.SelectedValue))
            {
                List<string> lista = Seleccion;
                lista.Remove(sel.Value);
                Seleccion = lista;
                RenderSeleccionNueva();
                return;
            }

            // Caso 2: sobre un perfil/familia EXISTENTE
            int id = int.Parse(ddlRolesFamilias.SelectedValue);
            if (ModoRol)
            {
                bllRol.DesasignarAccesoAlRol(id, acceso);
                RenderAccesos(bllRol.ObtenerAccesosPorRol(id));
            }
            else
            {
                bllFamilia.DesasignarAccesoAFamilia(id, acceso);
                Familia fam = bllFamilia.ObtenerFamiliaEspecifica(id);
                RenderAccesos(fam.ObtenerHijos());
            }

            CargarCatalogo();
            CargarPerfiles();
            Mostrar("Permiso/familia quitado correctamente.");
        }
        catch (Exception ex) { Mostrar(ex.Message); }
    }
    #endregion

    protected void btnLimpiar_Click(object sender, EventArgs e)
    {
        LimpiarFormulario();
        twPermisosSeleccionados.Nodes.Clear();
        pnlAlerta.Visible = false;
    }

    private void CargarCatalogo()
    {
        twPermisosFamilias.Nodes.Clear();

        // Raíz "Permisos" (patentes, Tipo 'S')
        TreeNode raizPermisos = new TreeNode("Permisos", string.Empty);
        raizPermisos.SelectAction = TreeNodeSelectAction.None;
        foreach (AccesoPermiso p in bllFamilia.ObtenerTodosLosPermisosSimples())
            raizPermisos.ChildNodes.Add(NuevoNodo(p));

        // Raíz "Familias" (Tipo 'C', con su árbol)
        TreeNode raizFamilias = new TreeNode("Familias", string.Empty);
        raizFamilias.SelectAction = TreeNodeSelectAction.None;
        foreach (AccesoPermiso acc in bllFamilia.ObtenerTodasLasFamilias())
        {
            Familia fam = (Familia)acc;
            TreeNode nodo = NuevoNodo(fam);
            AgregarHijos(nodo, fam);
            raizFamilias.ChildNodes.Add(nodo);
        }

        twPermisosFamilias.Nodes.Add(raizPermisos);
        twPermisosFamilias.Nodes.Add(raizFamilias);
        twPermisosFamilias.ExpandAll();
    }

    private void CargarPerfiles()
    {
        twRoles.Nodes.Clear();

        foreach (Rol rol in bllRol.ObtenerRoles())
        {
            TreeNode nodoRol = new TreeNode(rol.Nombre, "R:" + rol.ID);
            nodoRol.SelectAction = TreeNodeSelectAction.None;

            foreach (AccesoPermiso acc in bllRol.ObtenerAccesosPorRol(rol.ID))
            {
                TreeNode nodoAcc = NuevoNodo(acc);
                if (acc is Familia fam) AgregarHijos(nodoAcc, fam);
                nodoRol.ChildNodes.Add(nodoAcc);
            }

            twRoles.Nodes.Add(nodoRol);
        }

        twRoles.ExpandAll();
    }

    private void CargarCombo()
    {
        ddlRolesFamilias.Items.Clear();
        ddlRolesFamilias.Items.Add(new ListItem("-- Seleccionar --", string.Empty));

        if (ModoRol)
        {
            foreach (Rol rol in bllRol.ObtenerRoles())
                ddlRolesFamilias.Items.Add(new ListItem(rol.Nombre, rol.ID.ToString()));
        }
        else
        {
            foreach (AccesoPermiso acc in bllFamilia.ObtenerTodasLasFamilias())
                ddlRolesFamilias.Items.Add(new ListItem(acc.Nombre, acc.ID.ToString()));
        }
    }

    private void RenderAccesos(List<AccesoPermiso> accesos)
    {
        twPermisosSeleccionados.Nodes.Clear();
        foreach (AccesoPermiso acc in accesos)
        {
            TreeNode nodo = NuevoNodo(acc);
            if (acc is Familia fam) AgregarHijos(nodo, fam);
            twPermisosSeleccionados.Nodes.Add(nodo);
        }
        twPermisosSeleccionados.ExpandAll();
    }

    private void RenderSeleccionNueva()
    {
        List<AccesoPermiso> accesos = Seleccion.Select(ResolverAcceso).Where(a => a != null).ToList();
        RenderAccesos(accesos);
    }

    private void AgregarHijos(TreeNode nodo, Familia familia)
    {
        foreach (AccesoPermiso hijo in familia.ObtenerHijos())
        {
            TreeNode nodoHijo = NuevoNodo(hijo);
            if (hijo is Familia fam) AgregarHijos(nodoHijo, fam);
            nodo.ChildNodes.Add(nodoHijo);
        }
    }

    private TreeNode NuevoNodo(AccesoPermiso acceso)
    {
        return new TreeNode(acceso.Nombre, acceso.Tipo + ":" + acceso.ID);
    }

    private AccesoPermiso ResolverAcceso(string token)
    {
        if (string.IsNullOrEmpty(token) || !token.Contains(":")) return null;

        string[] partes = token.Split(':');
        char tipo = partes[0][0];
        int id = int.Parse(partes[1]);

        if (tipo == 'C')
            return bllFamilia.ObtenerFamiliaEspecifica(id);

        return bllFamilia.ObtenerTodosLosPermisosSimples().FirstOrDefault(p => p.ID == id);
    }

    private void AjustarModo()
    {
        lblLista.Text = ModoRol ? "Roles" : "Familias";
        CargarCombo();
    }

    private void RecargarTodo()
    {
        CargarCatalogo();
        CargarPerfiles();
        CargarCombo();
    }

    private void LimpiarFormulario()
    {
        txtNombre.Text = string.Empty;
        Seleccion = new List<string>();
        if (ddlRolesFamilias.Items.Count > 0)
            ddlRolesFamilias.SelectedIndex = 0;
    }

    private void Mostrar(string mensaje)
    {
        lbMensaje.Text = mensaje;
        pnlAlerta.Visible = true;
    }
}