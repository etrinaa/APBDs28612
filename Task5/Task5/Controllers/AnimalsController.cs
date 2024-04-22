using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Task5.Models;

[Route("api/[controller]")]
[ApiController]
public class AnimalsController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AnimalsController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    private List<Dictionary<string, object>> ConvertDataTableToList(DataTable dt)
    {
        var columns = dt.Columns.Cast<DataColumn>();
        return dt.Rows.Cast<DataRow>()
            .Select(row => columns.ToDictionary(column => column.ColumnName, column => row[column]))
            .ToList();
    }

    // GET: api/animals
    [HttpGet]
    public IActionResult GetAnimals(string orderBy = "name")
    {
        if (string.IsNullOrEmpty(orderBy) || !new[] { "name", "description", "category", "area" }.Contains(orderBy.ToLower()))
        {
            return BadRequest("Invalid sorting parameter");
        }

        string query = $"SELECT * FROM Animals ORDER BY {orderBy}";
        DataTable table = new DataTable();
        string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
        using (SqlConnection myCon = new SqlConnection(sqlDataSource))
        {
            myCon.Open();
            using (SqlCommand myCommand = new SqlCommand(query, myCon))
            {
                using (SqlDataReader myReader = myCommand.ExecuteReader())
                {
                    table.Load(myReader);
                }
            }
        }

        var list = ConvertDataTableToList(table);
        return Ok(list);
    }

    // POST: api/animals
    [HttpPost]
    public IActionResult PostAnimal(Animal animal)
    {
        if (animal == null || string.IsNullOrWhiteSpace(animal.Name))
        {
            return BadRequest("Animal data is incomplete");
        }

        string query = @"
           INSERT INTO Animals (Name, Description, Category, Area) 
           VALUES (@Name, @Description, @Category, @Area)";
        string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
        using (SqlConnection myCon = new SqlConnection(sqlDataSource))
        {
            myCon.Open();
            using (SqlCommand myCommand = new SqlCommand(query, myCon))
            {
                myCommand.Parameters.AddWithValue("@Name", animal.Name);
                myCommand.Parameters.AddWithValue("@Description", animal.Description);
                myCommand.Parameters.AddWithValue("@Category", animal.Category);
                myCommand.Parameters.AddWithValue("@Area", animal.Area);
                myCommand.ExecuteNonQuery();
            }
        }

        return Ok("Animal added successfully");
    }

    // PUT: api/animals/{idAnimal}
    [HttpPut("{idAnimal}")]
    public IActionResult UpdateAnimal(int idAnimal, Animal animal)
    {
        if (animal == null || idAnimal <= 0)
        {
            return BadRequest("Invalid data or ID");
        }

        string query = @"
            UPDATE Animals 
            SET Name = @Name, Description = @Description, Category = @Category, Area = @Area 
            WHERE IdAnimal = @IdAnimal";

        string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
        using (SqlConnection myCon = new SqlConnection(sqlDataSource))
        {
            myCon.Open();
            using (SqlCommand myCommand = new SqlCommand(query, myCon))
            {
                myCommand.Parameters.AddWithValue("@IdAnimal", idAnimal);
                myCommand.Parameters.AddWithValue("@Name", animal.Name);
                myCommand.Parameters.AddWithValue("@Description", animal.Description);
                myCommand.Parameters.AddWithValue("@Category", animal.Category);
                myCommand.Parameters.AddWithValue("@Area", animal.Area);
                var affectedRows = myCommand.ExecuteNonQuery();

                if (affectedRows == 0)
                {
                    return NotFound($"No animal found with ID: {idAnimal}");
                }
            }
        }

        return Ok("Animal updated successfully");
    }

    // DELETE: api/animals/{idAnimal}
    [HttpDelete("{idAnimal}")]
    public IActionResult DeleteAnimal(int idAnimal)
    {
        if (idAnimal <= 0)
        {
            return BadRequest("Invalid ID");
        }

        string query = "DELETE FROM Animals WHERE IdAnimal = @IdAnimal";

        string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
        using (SqlConnection myCon = new SqlConnection(sqlDataSource))
        {
            myCon.Open();
            using (SqlCommand myCommand = new SqlCommand(query, myCon))
            {
                myCommand.Parameters.AddWithValue("@IdAnimal", idAnimal);
                var affectedRows = myCommand.ExecuteNonQuery();

                if (affectedRows == 0)
                {
                    return NotFound($"No animal found with ID: {idAnimal}");
                }
            }
        }

        return Ok("Animal deleted successfully");
    }
}
