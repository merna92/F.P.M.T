using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using software_API.Models;

namespace software_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly string _connectionString;

        public UsersController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
        }

        [HttpPost("RegisterDonor")]
        public IActionResult RegisterDonor([FromBody] User u)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        // 1. إضافة في جدول User
                        string sqlUser = @"INSERT INTO [User] (FName, LName, Email, Password, IsVerified) 
                                         OUTPUT INSERTED.UserID VALUES (@f, @l, @e, @p, @v)";
                        SqlCommand cmdUser = new SqlCommand(sqlUser, conn, trans);
                        cmdUser.Parameters.AddWithValue("@f", u.FName ?? (object)DBNull.Value);
                        cmdUser.Parameters.AddWithValue("@l", u.LName ?? (object)DBNull.Value);
                        cmdUser.Parameters.AddWithValue("@e", u.Email ?? (object)DBNull.Value);
                        cmdUser.Parameters.AddWithValue("@p", u.Password ?? (object)DBNull.Value);
                        cmdUser.Parameters.AddWithValue("@v", u.IsVerified);

                        int newUserId = (int)cmdUser.ExecuteScalar();

                        // 2. إضافة في جدول Donor مربوط باليوزر الجديد
                        string sqlDonor = "INSERT INTO Donor (DonorID, Donation_Count) VALUES (@did, 0)";
                        SqlCommand cmdDonor = new SqlCommand(sqlDonor, conn, trans);
                        cmdDonor.Parameters.AddWithValue("@did", newUserId);
                        cmdDonor.ExecuteNonQuery();

                        trans.Commit();
                        return Ok(new { Message = "Success", UserID = newUserId });
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return BadRequest(ex.Message);
                    }
                }
            }
        }
    }
}