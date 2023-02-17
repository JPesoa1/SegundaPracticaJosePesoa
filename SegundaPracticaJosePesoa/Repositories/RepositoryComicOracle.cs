using Oracle.ManagedDataAccess.Client;
using SegundaPracticaJosePesoa.Models;
using System.Data;

namespace SegundaPracticaJosePesoa.Repositories
{

    #region
    //    CREATE OR REPLACE PROCEDURE SP_INSERT_COMIC
    //(IDCOMIC COMICS.IDCOMIC%type, NOMBRE COMICS.NOMBRE%TYPE
    //, IMAGEN COMICS.IMAGEN%TYPE, DESCRIPCION COMICS.DESCRIPCION%TYPE)
    //AS
    //BEGIN
    //    INSERT INTO COMICS VALUES
    //  (IDCOMIC , NOMBRE
    //  , IMAGEN , DESCRIPCION);
    //    COMMIT;
    //END;
    #endregion
    public class RepositoryComicOracle : IRepository
    {

        private OracleConnection cn;
        private OracleCommand com;
        private OracleDataAdapter adapter;
        private DataTable tablaComics;

        public RepositoryComicOracle()
        {

            string connectionString =
                 @"Data Source=LOCALHOST:1521/XE; Persist Security Info=True;User Id=SYSTEM;Password=oracle";
            this.cn = new OracleConnection(connectionString);
            this.com = new OracleCommand();
            this.com.Connection = this.cn;
            string sql = "select * from comics";
            this.adapter = new OracleDataAdapter(sql, this.cn);
            this.tablaComics = new DataTable();
            this.adapter.Fill(this.tablaComics);
        }

        public List<Comic> GetComics()
        {
            var consulta = from datos in this.tablaComics.AsEnumerable()
                           select new Comic
                           {
                               IdComic = datos.Field<int>("IDCOMIC"),
                               Nombre = datos.Field<string>("NOMBRE"),
                               Imagen = datos.Field<string>("IMAGEN"),
                               Descripcion = datos.Field<string>("DESCRIPCION"),
                           };

            return consulta.ToList();

        }

        private int GetMaxIdComic()
        {
            int maximo = (from datos in this.tablaComics.AsEnumerable()
                          select datos).Max(z => z.Field<int>("IDCOMIC")) + 1;
            return maximo;
        }

        public void Insertar(int id, string nombre, string imagen, string descripcion)
        {
            int maximo = this.GetMaxIdComic();
            OracleParameter pamid = new OracleParameter("IDCOMIC", maximo);
            this.com.Parameters.Add(pamid);
            OracleParameter pamnombre = new OracleParameter("NOMBRE", nombre);
            this.com.Parameters.Add(pamnombre);
            OracleParameter pamimagen = new OracleParameter("IMAGEN", imagen);
            this.com.Parameters.Add(pamimagen);
            OracleParameter pamdesc = new OracleParameter("DESCRIPCION", descripcion);
            this.com.Parameters.Add(pamdesc);

            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_INSERT_COMIC";
            this.cn.Open();
            this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }
    }
}
