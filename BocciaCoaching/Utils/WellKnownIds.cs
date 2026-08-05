namespace BocciaCoaching.Utils
{
    /// <summary>
    /// ES: Identificadores fijos (GUID) para filas sembradas conocidas.
    /// EN: Well-known seeded row GUIDs.
    /// </summary>
    public static class WellKnownIds
    {
        /// <summary>Módulo de error general (antes ModuleErrorId = 1).</summary>
        public static readonly Guid GeneralModule = new("00000000-0000-0000-0000-0000000000A1");

        /// <summary>Tipo de notificación general (antes NotificationTypeId = 1).</summary>
        public static readonly Guid NotificationTypeGeneral = new("00000000-0000-0000-0000-0000000000B1");

        /// <summary>Tipo de notificación: invitación a equipo (antes NotificationTypeId = 2).</summary>
        public static readonly Guid NotificationTypeTeamInvitation = new("00000000-0000-0000-0000-0000000000B2");
    }
}


