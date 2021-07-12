using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseInteraction.Models
{
    /// <summary>
    /// The class for storing a picture from the database
    /// </summary>
    public class Picture
    {

        #region Private Fields

        private Guid m_imageID;
        private string m_imageName;
        private byte[] m_image;
        private bool m_isMain;
        #endregion


        #region Public Properties

        /// <summary>
        /// The image ID in the database
        /// </summary>
        public Guid ImageID
        {
            get { return m_imageID; }
            set { m_imageID = value; }
        }

        /// <summary>
        /// The name of the image
        /// </summary>
        public string ImageName
        {
            get { return m_imageName; }
            set { m_imageName = value; }
        }

        /// <summary>
        /// Byte array containing the image blob
        /// </summary>
        public byte[] Image
        {
            get { return m_image; }
            set { m_image = value; }
        }



        /// <summary>
        /// Specifies whether the picture is the main image
        /// </summary>
        public bool IsMain
        {
            get { return m_isMain; }
            set { m_isMain = value; }
        }

        public bool IsValid
        {
            get
            {
                return ValidateString(ImageName);
            }
        }
        private bool ValidateString(string value)
        {
            return (!string.IsNullOrWhiteSpace(value) && value != "");
        }
        #endregion

        #region Constructors

        public Picture(Guid imageID, string imageName, byte[] image)
        {
            m_imageID = imageID;
            ImageName = imageName;
            Image = image;

        }
        public Picture() { }
        #endregion


        public void AddImageToDatabase(long productID)
        {
            // todo: implement this!! need stored procedure
            throw new NotImplementedException();
        }


        public static Picture GetImageFromDatabase(Guid imageID)
        {
            SqlConnection sqlConn = null;
            List<Picture> result = null;
            try
            {
                if (imageID == Guid.Empty) return null;
                if (!Database.ConnectToDatabase(ref sqlConn)) return null;


                string strSqlCommand = $"SELECT * FROM TImages WHERE lngImageID = {imageID}";


                SqlCommand sqlCommand = new SqlCommand(strSqlCommand, sqlConn);

                SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
                DataTable dt = new DataTable();
                da.Fill(dt);

                result = new List<Picture>();
                foreach (DataRow dr in dt.Rows)
                {

                    Guid rowImageID = (Guid)(dr["guidImageID"]);
                    string imageName = dr["strImageName"].ToString();
                    byte[] image = (byte[])dr["blbImage"];
                    result.Add(new Picture(rowImageID, imageName, image));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                result = null;
            }
            finally
            {
                if (sqlConn != null && sqlConn.State == ConnectionState.Open)
                    sqlConn.Close();
            }

            return result[0];
        }
    }
}
