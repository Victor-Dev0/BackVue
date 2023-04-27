using Cadastro.Data;
using Cadastro.Models;
using Cadastro.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cadastro.Repositorios
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly AppDbContext _appDbContext;
        public UsuarioRepositorio(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Usuario> Adicionar(Usuario usuario)
        {
            try
            {
                await _appDbContext.Usuarios.AddAsync(usuario);
                await _appDbContext.SaveChangesAsync();

                return usuario;
            }
            catch (Exception e)
            {

                throw new Exception($"Ouve um erro: ${ e.Message }");
            }
        }

        public async Task<Usuario> Atualizar(Usuario usuario, int id)
        {
            Usuario usuarioPorId = await BuscarPorId(id);

            if (usuarioPorId == null)
            {
                throw new Exception($"Usuario do ID: ${id}, não foi encontrado!");
            }

            usuarioPorId.Email = usuario.Email;
            usuarioPorId.Senha = usuario.Senha;

            _appDbContext.Usuarios.Update(usuarioPorId);
            await _appDbContext.SaveChangesAsync();

            return usuarioPorId;
        }

        public async Task<Usuario> BuscarPorId(int id)
        {
            try
            {
                return await _appDbContext.Usuarios.FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception)
            {

                throw new Exception($"O Usuario para o ID: ${ id } não foi encontrado no banco de dados!");
            }
        }

        public async Task<List<Usuario>> BuscarTodos()
        {
            try
            {
                return await _appDbContext.Usuarios.ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> Apagar(int id)
        {
            Usuario usuarioPorId = await BuscarPorId(id);

            if (usuarioPorId == null)
            {
                throw new Exception($"Usuario do ID: ${id}, não foi encontrado!");
            }

            _appDbContext.Usuarios.Remove(usuarioPorId);
            await _appDbContext.SaveChangesAsync();

            return true;
        }
    }
}
