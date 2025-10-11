using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Data
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) { }

        public DbSet<Author> Authors => Set<Author>();
        public DbSet<Book> Books => Set<Book>();
        public DbSet<Loan> Loans => Set<Loan>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LibraryDbContext).Assembly);

            //SeedData(modelBuilder);
        }
    


    /// <summary>
    /// Método para agregar datos de prueba iniciales
    /// </summary>
    private void SeedData(ModelBuilder modelBuilder)
    {
        // Autores de ejemplo
        modelBuilder.Entity<Author>().HasData(
            new Author { Autor_id = 1, Nombre = "Gabriel García Márquez", Nacionalidad = "Colombiano" },
            new Author { Autor_id = 2, Nombre = "Jorge Luis Borges", Nacionalidad = "Argentino" },
            new Author { Autor_id = 3, Nombre = "Isabel Allende", Nacionalidad = "Chilena" },
            new Author { Autor_id = 4, Nombre = "Mario Vargas Llosa", Nacionalidad = "Peruano" },
            new Author { Autor_id = 5, Nombre = "Octavio Paz", Nacionalidad = "Mexicano" }
        );

        // Libros de ejemplo (algunos antes del 2000)
        modelBuilder.Entity<Book>().HasData(
            new Book { Libro_id = 1, Titulo = "Cien años de soledad", Autor_id = 1, Año_publicacion = 1967, Genero = "Realismo mágico" },
            new Book { Libro_id = 2, Titulo = "El amor en los tiempos del cólera", Autor_id = 1, Año_publicacion = 1985, Genero = "Romance" },
            new Book { Libro_id = 3, Titulo = "Ficciones", Autor_id = 2, Año_publicacion = 1944, Genero = "Ficción" },
            new Book { Libro_id = 4, Titulo = "El Aleph", Autor_id = 2, Año_publicacion = 1949, Genero = "Ficción" },
            new Book { Libro_id = 5, Titulo = "La casa de los espíritus", Autor_id = 3, Año_publicacion = 1982, Genero = "Realismo mágico" },
            new Book { Libro_id = 6, Titulo = "La ciudad y los perros", Autor_id = 4, Año_publicacion = 1963, Genero = "Novela" },
            new Book { Libro_id = 7, Titulo = "Conversación en La Catedral", Autor_id = 4, Año_publicacion = 1969, Genero = "Novela" },
            new Book { Libro_id = 8, Titulo = "El laberinto de la soledad", Autor_id = 5, Año_publicacion = 1950, Genero = "Ensayo" },
            new Book { Libro_id = 9, Titulo = "Inés del alma mía", Autor_id = 3, Año_publicacion = 2006, Genero = "Novela histórica" },
            new Book { Libro_id = 10, Titulo = "Memorias de mis putas tristes", Autor_id = 1, Año_publicacion = 2004, Genero = "Novela" }
        );

        // Préstamos de ejemplo (algunos sin devolver)
        modelBuilder.Entity<Loan>().HasData(
            new Loan { Prestamo_id = 1, Libro_id = 1, Fecha_prestamo = new DateTime(2024, 1, 15), Fecha_devolucion = new DateTime(2024, 2, 1) },
            new Loan { Prestamo_id = 2, Libro_id = 3, Fecha_prestamo = new DateTime(2024, 2, 10), Fecha_devolucion = null }, // No devuelto
            new Loan { Prestamo_id = 3, Libro_id = 5, Fecha_prestamo = new DateTime(2024, 3, 5), Fecha_devolucion = new DateTime(2024, 3, 20) },
            new Loan { Prestamo_id = 4, Libro_id = 2, Fecha_prestamo = new DateTime(2024, 4, 1), Fecha_devolucion = null }, // No devuelto
            new Loan { Prestamo_id = 5, Libro_id = 7, Fecha_prestamo = new DateTime(2024, 5, 12), Fecha_devolucion = null }, // No devuelto
            new Loan { Prestamo_id = 6, Libro_id = 9, Fecha_prestamo = new DateTime(2024, 6, 8), Fecha_devolucion = new DateTime(2024, 6, 22) }
        );
    }
    }
}