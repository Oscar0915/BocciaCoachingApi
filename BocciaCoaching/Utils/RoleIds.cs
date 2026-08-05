namespace BocciaCoaching.Utils
{
    /// <summary>
    /// ES: Identificadores fijos (GUID) de los roles del sistema.
    /// Reemplazan los antiguos IDs enteros (1=Admin, 2=Coach, 3=Atleta).
    /// Se siembran en la base de datos mediante HasData en ApplicationDbContext.
    /// EN: Well-known role GUIDs. They replace the old integer role ids.
    /// </summary>
    public static class RoleIds
    {
        public static readonly Guid Admin = new("00000000-0000-0000-0000-000000000001");
        public static readonly Guid Coach = new("00000000-0000-0000-0000-000000000002");
        public static readonly Guid Athlete = new("00000000-0000-0000-0000-000000000003");
    }
}

