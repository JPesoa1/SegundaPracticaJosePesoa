using SegundaPracticaJosePesoa.Models;
using System.Data;
using System.Data.SqlClient;

namespace SegundaPracticaJosePesoa.Repositories
{


    #region
        //    CREATE PROCEDURE SP_INSERT_COMIC
        //(@IDCOMIC INT, @NOMBRE NVARCHAR(50)
        //, @IMAGEN NVARCHAR(50), @DESCRIPCION NVARCHAR(20))
        //AS
        //    INSERT INTO COMICS VALUES
        //    (@IDCOMIC , @NOMBRE

        //    , @IMAGEN , @DESCRIPCION)
        //GO
    #endregion
    public class RepositoryComicSQL : IRepository
    {

        private SqlConnection cn;
        private SqlCommand com;
        private SqlDataAdapter adapter;
        private DataTable tablaComic;

        public RepositoryComicSQL()
        {
            string connectionString =
              @"Data Source=LOCALHOST\DESARROLLO;Initial Catalog=HOSPITAL;Persist Security Info=True;User ID=SA;Password=MCSD2023";
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
            string sql = "select * from comics";
            this.adapter = new SqlDataAdapter(sql, this.cn);
            this.tablaComic = new DataTable();
            this.adapter.Fill(this.tablaComic);
        }

        public List<Comic> GetComics()
        {

            var consulta = from datos in this.tablaComic.AsEnumerable()
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
            int maximo = (from datos in this.tablaComic.AsEnumerable()
                          select datos).Max(z => z.Field<int>("IDCOMIC")) + 1;
            return maximo;
        }


        public void Insertar(int id, string nombre, string imagen, string descripcion)
        {

            int maximo = this.GetMaxIdComic();
            SqlParameter pamid = new SqlParameter("@IDCOMIC", maximo);
            this.com.Parameters.Add(pamid);
            SqlParameter pamnombre = new SqlParameter("@NOMBRE", nombre);
            this.com.Parameters.Add(pamnombre);
            SqlParameter pamimagen = new SqlParameter("@IMAGEN", imagen);
            this.com.Parameters.Add(pamimagen);
            SqlParameter pamdesc = new SqlParameter("@DESCRIPCION", descripcion);
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
