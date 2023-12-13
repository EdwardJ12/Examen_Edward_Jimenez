using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;





namespace ApiMecanico.Controllers
{
    [Route("api/[controller]")]
    public class ApiMecanicoController : ControllerBase
    {
        private readonly string StringConector;

        public ApiMecanicoController(IConfiguration config)
        {
            StringConector = config?.GetConnectionString("MySqlConnection") ?? throw new ArgumentNullException(nameof(config));
        }
        //enlistar personas por id
        [HttpGet("{idMecanico}")]
        public ActionResult<Mecanico> ObtenerDatosMecanico(int idMecanico)
        {
            try
            {
                using (MySqlConnection conecta = new MySqlConnection(StringConector))
                {
                    conecta.Open();

                    string query = "SELECT * FROM Mecanicos WHERE Id_mecanico = @IdMecanico";

                    using (MySqlCommand comandos = new MySqlCommand(query, conecta))
                    {
                        comandos.Parameters.AddWithValue("@IdMecanico", idMecanico);

                        MySqlDataReader lector = comandos.ExecuteReader();

                        if (lector.Read())
                        {
                            var mecanico = new Mecanico
                            {
                                Id_mecanico = lector.GetInt32(0),
                                nombre = lector.GetString(1),
                                Edad = lector.GetInt32(2),
                                Domicilio = lector.GetString(3),
                                Titulo = lector.GetString(4),
                                Especialidad = lector.GetString(5),
                                SueldoBase = lector.GetInt32(6),
                                GranTitulo = lector.GetInt32(7),
                                SueldoTotal = lector.GetInt32(8)
                            };

                            return Ok(mecanico);
                        }
                        else
                        {
                            return NotFound(new { mensaje = "Mecánico no encontrado" });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
        //metodo para enlistar personas
        [HttpGet]
        public ActionResult<IEnumerable<Mecanico>> ListarMecanicos()
        {
            try
            {
                using (MySqlConnection conecta = new MySqlConnection(StringConector))
                {
                    conecta.Open();

                    string query = "SELECT * FROM Mecanicos";

                    using (MySqlCommand comandos = new MySqlCommand(query, conecta))
                    {
                        MySqlDataReader lector = comandos.ExecuteReader();

                        var mecanicos = new List<Mecanico>();

                        while (lector.Read())
                        {
                            var mecanico = new Mecanico
                            {
                                Id_mecanico = lector.GetInt32(0),
                                nombre = lector.GetString(1),
                                Edad = lector.GetInt32(2),
                                Domicilio = lector.GetString(3),
                                Titulo = lector.GetString(4),
                                Especialidad = lector.GetString(5),
                                SueldoBase = lector.GetInt32(6),
                                GranTitulo = lector.GetInt32(7),
                                SueldoTotal = lector.GetInt32(8)
                            };

                            mecanicos.Add(mecanico);
                        }

                        return Ok(mecanicos);
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        //creamos el método que nos permitirá crear una nueva persona
        [HttpPost]
        public IActionResult AgregarMecanico([FromBody] Mecanico nuevoMecanico)
        {
            try
            {
                using (MySqlConnection conecta = new MySqlConnection(StringConector))
                {
                    conecta.Open();

                    //se realiza esta variable para que cuando uno inserte el sueldo base + el titulo que le da el dinero extra se sume automaticamente en Sueldo total
                    nuevoMecanico.SueldoTotal = nuevoMecanico.SueldoBase + nuevoMecanico.GranTitulo;

                    string query = "INSERT INTO Mecanicos (nombre, Edad, Domicilio, Titulo, Especialidad, SueldoBase, GranTitulo, SueldoTotal) " +
                                   "VALUES (@nombre, @Edad, @Domicilio, @Titulo, @Especialidad, @SueldoBase, @GranTitulo, @SueldoTotal)";

                    using (MySqlCommand comandos = new MySqlCommand(query, conecta))
                    {
                        comandos.Parameters.AddWithValue("@nombre", nuevoMecanico.nombre);
                        comandos.Parameters.AddWithValue("@Edad", nuevoMecanico.Edad);
                        comandos.Parameters.AddWithValue("@Domicilio", nuevoMecanico.Domicilio);
                        comandos.Parameters.AddWithValue("@Titulo", nuevoMecanico.Titulo);
                        comandos.Parameters.AddWithValue("@Especialidad", nuevoMecanico.Especialidad);
                        comandos.Parameters.AddWithValue("@SueldoBase", nuevoMecanico.SueldoBase);
                        comandos.Parameters.AddWithValue("@GranTitulo", nuevoMecanico.GranTitulo);
                        comandos.Parameters.AddWithValue("@SueldoTotal", nuevoMecanico.SueldoTotal);

                        comandos.ExecuteNonQuery();
                    }
                }
               

                return StatusCode(201, "Mecánico agregado con éxito");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        //creamos el metodo que nos permitirá editar una persona
        [HttpPut("{idMecanico}")]
        public IActionResult ActualizarMecanico(int idMecanico, [FromBody] Mecanico mecanicoActualizado)
        {
            try
            {
                using (MySqlConnection conecta = new MySqlConnection(StringConector))
                {
                    conecta.Open();

                    string query = "UPDATE Mecanicos SET nombre = @nombre, Edad = @Edad, Domicilio = @Domicilio, " +
                                   "Titulo = @Titulo, Especialidad = @Especialidad, SueldoBase = @SueldoBase, " +
                                   "GranTitulo = @GranTitulo, SueldoTotal = @SueldoTotal WHERE Id_mecanico = @IdMecanico";

                    using (MySqlCommand comandos = new MySqlCommand(query, conecta))
                    {
                        comandos.Parameters.AddWithValue("@IdMecanico", idMecanico);
                        comandos.Parameters.AddWithValue("@nombre", mecanicoActualizado.nombre);
                        comandos.Parameters.AddWithValue("@Edad", mecanicoActualizado.Edad);
                        comandos.Parameters.AddWithValue("@Domicilio", mecanicoActualizado.Domicilio);
                        comandos.Parameters.AddWithValue("@Titulo", mecanicoActualizado.Titulo);
                        comandos.Parameters.AddWithValue("@Especialidad", mecanicoActualizado.Especialidad);
                        comandos.Parameters.AddWithValue("@SueldoBase", mecanicoActualizado.SueldoBase);
                        comandos.Parameters.AddWithValue("@GranTitulo", mecanicoActualizado.GranTitulo);
                        comandos.Parameters.AddWithValue("@SueldoTotal", mecanicoActualizado.SueldoTotal);

                        int filasAfectadas = comandos.ExecuteNonQuery();

                        if (filasAfectadas > 0)
                        {
                            mecanicoActualizado.SueldoTotal = mecanicoActualizado.SueldoBase + mecanicoActualizado.GranTitulo;

                            return Ok($"Mecánico con ID {idMecanico} actualizado exitosamente");
                        }
                        else
                        {
                            return NotFound($"Mecánico con ID {idMecanico} no encontrado");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        //creamos el método que nos permitirá eliminar una persona
        [HttpDelete("{idMecanico}")]
        public IActionResult EliminarMecanico(int idMecanico)
        {
            try
            {
                using (MySqlConnection conecta = new MySqlConnection(StringConector))
                {
                    conecta.Open();

                    string query = "DELETE FROM Mecanicos WHERE Id_mecanico = @IdMecanico";

                    using (MySqlCommand comandos = new MySqlCommand(query, conecta))
                    {
                        comandos.Parameters.AddWithValue("@IdMecanico", idMecanico);

                        int filasAfectadas = comandos.ExecuteNonQuery();

                        if (filasAfectadas > 0)
                        {
                            //retornamos un mensaje de confirmación
                            return Ok($"Mecánico con ID {idMecanico} eliminado exitosamente");
                        }
                        else
                        {
                            return NotFound($"Mecánico con ID {idMecanico} no encontrado");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
