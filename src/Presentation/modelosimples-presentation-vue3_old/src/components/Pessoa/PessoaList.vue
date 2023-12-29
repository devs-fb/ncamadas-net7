<template>
  <div>
    <h1>Lista de Pessoas</h1>
    <VueClientTable :data="pessoas" :columns="columns" :options="options">
      <template v-slot:nome="{ row }">
        <router-link :to="{ name: 'PessoaDetails', params: { id: row.id } }">
          {{ row.pessoaFisica?.nomeSocial || row.pessoaJuridica?.nomeFantasia }}
        </router-link>
      </template>
    </VueClientTable>
  </div>
</template>

<script>
import VueClientTable from 'vue-tables-3';
import PessoaService from '../api/PessoaService';

export default {
  components: {
    VueClientTable,
  },
  data() {
    return {
      pessoas: [],
      columns: ['id', 'nome'], // Defina as colunas a serem exibidas na tabela
      options: {
        headings: {
          id: 'ID',
          nome: 'Nome', // Defina os cabeçalhos das colunas
        },
        sortable: ['id', 'nome'], // Defina as colunas que podem ser ordenadas
        filterable: ['nome'], // Defina as colunas que podem ser filtradas
        perPage: 10, // Itens por página
        perPageValues: [5, 10, 15, 20], // Opções de itens por página
        pagination: {
          edge: true,
        },
      },
    };
  },
  async mounted() {
    await this.listarPessoas();
  },
  methods: {
    async listarPessoas() {
      try {
        const filtros = {
          paginacaoOrdenacao: {
            paginacao: {
              pagina: this.options.page,
              tamanhoPagina: this.options.perPage,
              totalRegistro: 0,
            },
            ordenacao: [
              {
                campo: this.options.sortBy,
                ascendente: this.options.sortOrder === 'asc',
              },
            ],
          },
          pessoa: {
            id: '', // Se tiver um ID específico para buscar, adicione aqui
            // Outros campos de filtro conforme necessário
          },
        };

        const response = await PessoaService.buscarPessoa(filtros);
        this.pessoas = response;
      } catch (error) {
        console.error('Erro ao buscar pessoas:', error);
      }
    },
  },
};
</script>

<style>
/* Estilos específicos para o componente PessoaList.vue */
</style>
