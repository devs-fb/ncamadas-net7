import http from 'axios';

const API_URL = 'https://localhost:21001'; 

export default class PessoaService {
  async createPessoa(pessoa) {
    try {
      const response = await http.post(`${API_URL}/api/Pessoa/Criar`, pessoa);
      return response.data;
    } catch (error) {
      throw new Error('Erro ao criar pessoa: ' + error.message);
    }
  }

  async editPessoa(pessoa) {
    try {
      const response = await http.put(`${API_URL}/api/Pessoa/Editar?id=${pessoa.id}`, pessoa);
      return response.data;
    } catch (error) {
      throw new Error('Erro ao editar pessoa: ' + error.message);
    }
  }

  async buscarPessoa(pessoaQuery) {
    try {
      const response = await http.post(`${API_URL}/api/Pessoa/Buscar`, pessoaQuery);
      return response.data;
    } catch (error) {
      throw new Error('Erro ao buscar pessoa: ' + error.message);
    }
  }

  async getPessoas(pagina, tamanhoPagina) {
    try {
      const pessoaQuery = {
        paginacaoOrdenacao: {
          paginacao: {
            pagina: pagina,
            tamanhoPagina: tamanhoPagina
          }
        }
      };
      
      const response = await http.post(`${API_URL}/api/Pessoa/Buscar`, pessoaQuery);
      console.log(response.data.dados);
      if (response.data.sucesso)
      {
        return response.data.dados;
      }
      return [];
    } catch (error) {
      throw new Error('Erro ao obter pessoa por ID: ' + error.message);
    }
  }

  async getPessoaById(pessoaId) {
    try {
      const response = await http.get(`${API_URL}/api/Pessoa/${pessoaId}`);
      return response.data;
    } catch (error) {
      throw new Error('Erro ao obter pessoa por ID: ' + error.message);
    }
  }

  async deletePessoa(pessoaId) {
    try {
      const response = await http.delete(`${API_URL}/api/Pessoa/${pessoaId}`);
      return response.data;
    } catch (error) {
      throw new Error('Erro ao excluir pessoa: ' + error.message);
    }
  }

  async auditarPessoa(pessoaId) {
    try {
      const response = await http.get(`${API_URL}/api/Pessoa/Auditar/${pessoaId}`);
      return response.data;
    } catch (error) {
      throw new Error('Erro ao auditar pessoa: ' + error.message);
    }
  }

  async bloquearPessoa(pessoaId) {
    try {
      const response = await http.post(`${API_URL}/api/Pessoa/Bloquear?pessoaId=${pessoaId}`);
      return response.data;
    } catch (error) {
      throw new Error('Erro ao bloquear pessoa: ' + error.message);
    }
  }

  async desbloquearPessoa(pessoaId) {
    try {
      const response = await http.post(`${API_URL}/api/Pessoa/Desbloquear?pessoaId=${pessoaId}`);
      return response.data;
    } catch (error) {
      throw new Error('Erro ao desbloquear pessoa: ' + error.message);
    }
  }
}
