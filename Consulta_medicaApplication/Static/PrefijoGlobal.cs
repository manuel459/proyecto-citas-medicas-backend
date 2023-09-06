namespace Consulta_medica.Static
{
    public static class DynamicPolicies
    {
        public const string POLICY_PREFIX = "PERMISSION_";
        public const string SEPARATOR = "_";
    }

    public static class Permissions
    {
        #region PERMISOS PARA EL MODULO DE MEDICOS
        public const string LIST_MODULE_MEDICOS = "LIST-MODULE-MEDICOS";
        public const string LIST_MODULE_MEDICOS_INDIVIDUAL = "LIST-MODULE-MEDICOS-INDIVIDUAL";
        public const string BUTTON_CREATE_MEDICO = "BUTTON-CREATE-MEDICO";
        public const string BUTTON_EDIT_MEDICO = "BUTTON-EDIT-MEDICO";
        public const string BUTTON_DELETE_MEDICO = "BUTTON-DELETE-MEDICO";
        #endregion

        #region PERMISOS PARA EL MODULO DE CITAS MEDICAS
        public const string LIST_MODULE_NEWCITA = "LIST-MODULE-NEWCITA";
        public const string LIST_MODULE_NEWCITA_PACIENTES = "LIST-MODULE-NEWCITA-PACIENTES";
        public const string BUTTON_CREATE_CITA = "BUTTON-CREATE-CITA";
        public const string VIEW_EDIT_CITA = "VIEW-EDIT-CITA";
        public const string VIEW_DELETE_CITA = "VIEW-DELETE-CITA";
        public const string GENERAR_DIAGNOSTICO = "GENERAR-DIAGNOSTICO";
        #endregion

        #region PERMISOS PARA EL MODULO DE PACIENTES
        public const string LIST_MODULE_PACIENTES_INDIVIDUAL = "LIST-MODULE-PACIENTES-INDIVIDUAL";
        public const string LIST_MODULE_PACIENTES = "LIST-MODULE-PACIENTES";
        public const string BUTTON_CREATE_PACIENTE = "BUTTON-CREATE-PACIENTE";
        public const string BUTTON_EDIT_PACIENTE = "BUTTON-EDIT-PACIENTE";
        public const string BUTTON_DELETE_PACIENTE = "BUTTON-DELETE-PACIENTE";
        #endregion

        #region PERMISOS PARA EL MODULO DE USUARIOS
        public const string LIST_MODULE_USER = "LIST-MODULE-USUARIOS";
        public const string LIST_MODULE_USER_INDIVIDUAL = "LIST-MODULE-USUARIOS-INDIVIDUAL";
        public const string BUTTON_CREATE_USUARIOS = "BUTTON-CREATE-USUARIOS";
        public const string BUTTON_EDIT_USUARIOS = "BUTTON-EDIT-USUARIOS";
        public const string BUTTON_DELETE_USUARIOS = "BUTTON-DELETE-USUARIOS";
        #endregion
    }

    public static class AppClaimTypes
    {
        public const string Permissions = "permissions";
    }

    public static class JobName 
    {
        public const string NOTI_RECORDATORIO_CITA = "Noti_Recordatorio_Cita";
    }
}
