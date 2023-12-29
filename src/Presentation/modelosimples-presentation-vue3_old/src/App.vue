<template>
  <div id="app">
    <h1>Lista de Pessoas</h1>
    <PessoaList :pessoas="pessoas" @detalhesPessoa="exibirDetalhes" />
    <div v-if="pessoaSelecionada">
      <h2>Detalhes da Pessoa</h2>
      <PessoaDetails :pessoa="pessoaSelecionada" @fecharDetalhes="fecharDetalhes" />
    </div>
  </div>
</template>

<script>
import PessoaList from './components/Pessoa/PessoaList.vue';
import PessoaDetails from './components/Pessoa/PessoaDetails.vue';
import PessoaService from './api/PessoaService.js';

export default {
  components: {
    PessoaList,
    PessoaDetails,
  },
  data() {
    return {
      pessoas: [],
      pessoaSelecionada: null,
    };
  },
  async created() {
    try {
      this.pessoas = await PessoaService.listarPessoas();
    } catch (error) {
      console.error('Erro ao buscar lista de pessoas:', error);
    }
  },
  methods: {
    async exibirDetalhes(pessoaId) {
      try {
        this.pessoaSelecionada = await PessoaService.detalhesPessoa(pessoaId);
      } catch (error) {
        console.error('Erro ao buscar detalhes da pessoa:', error);
      }
    },
    fecharDetalhes() {
      this.pessoaSelecionada = null;
    },
  },
};
</script>

<style>
/* Estilos globais para a aplicação */
</style>
