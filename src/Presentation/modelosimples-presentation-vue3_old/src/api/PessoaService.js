// src/api/PessoaService.js
import axios from 'axios';

const baseURL = 'https://localhost:21001';

const api = axios.create({
  baseURL,
});

export default {
  async buscarPessoa(filtros) {
    try {
      const response = await api.post('/api/Pessoa/Buscar', filtros);
      return response.data;
    } catch (error) {
      throw new Error('Erro ao buscar pessoas:', error);
    }
  },
  async obterPessoa(id) {
    try {
      const response = await api.get(`/api/Pessoa/${id}`);
      return response.data;
    } catch (error) {
      throw new Error('Erro ao obter pessoa:', error);
    }
  },
  async criarPessoa(pessoa) {
    try {
      const response = await api.post('/api/Pessoa/Criar', pessoa);
      return response.data;
    } catch (error) {
      throw new Error('Erro ao criar pessoa:', error);
    }
  },
  async editarPessoa(id, pessoa) {
    try {
      const response = await api.put(`/api/Pessoa/Editar?id=${id}`, pessoa);
      return response.data;
    } catch (error) {
      throw new Error('Erro ao editar pessoa:', error);
    }
  },
  async excluirPessoa(id) {
    try {
      const response = await api.delete(`/api/Pessoa/${id}`);
      return response.data;
    } catch (error) {
      throw new Error('Erro ao excluir pessoa:', error);
    }
  },
  async auditarPessoa(id) {
    try {
      const response = await api.get(`/api/Pessoa/Auditar/${id}`);
      return response.data;
    } catch (error) {
      throw new Error('Erro ao auditar pessoa:', error);
    }
  },
  async bloquearPessoa(id) {
    try {
      const response = await api.post(`/api/Pessoa/Bloquear?pessoaId=${id}`);
      return response.data;
    } catch (error) {
      throw new Error('Erro ao bloquear pessoa:', error);
    }
  },
  async desbloquearPessoa(id) {
    try {
      const response = await api.post(`/api/Pessoa/Desbloquear?pessoaId=${id}`);
      return response.data;
    } catch (error) {
      throw new Error('Erro ao desbloquear pessoa:', error);
    }
  }
};
