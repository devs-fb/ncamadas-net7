<template>
    <div>
      <h2>Detalhes da Pessoa</h2>
      <div v-if="pessoa">
        <p><strong>ID:</strong> {{ pessoa.id }}</p>
        <p v-if="pessoa.tipo === 'Fisica'">
          <strong>Nome Social:</strong> {{ pessoa.pessoaFisica.nomeSocial }}
        </p>
        <p v-if="pessoa.tipo === 'Fisica'">
          <strong>Data de Nascimento:</strong> {{ pessoa.pessoaFisica.dataNascimento }}
        </p>
        <p v-if="pessoa.tipo === 'Fisica'">
          <strong>Gênero:</strong> {{ pessoa.pessoaFisica.genero }}
        </p>
        <p v-if="pessoa.tipo === 'Juridica'">
          <strong>Razão Social:</strong> {{ pessoa.pessoaJuridica.razaoSocial }}
        </p>
        <p v-if="pessoa.tipo === 'Juridica'">
          <strong>Nome Fantasia:</strong> {{ pessoa.pessoaJuridica.nomeFantasia }}
        </p>
        <p v-if="pessoa.tipo === 'Juridica'">
          <strong>CNAE:</strong> {{ pessoa.pessoaJuridica.cnae }}
        </p>
        <button @click="fecharDetalhes">Fechar</button>
      </div>
      <div v-else>
        <p>Nenhum detalhe disponível.</p>
      </div>
    </div>
  </template>
  
  <script>
  export default {
    props: {
      pessoa: Object, // Recebe os detalhes da pessoa selecionada como propriedade
    },
    methods: {
      fecharDetalhes() {
        // Método para emitir um evento para o componente pai, indicando o fechamento dos detalhes
        this.$emit('fecharDetalhes');
      },
    },
  };
  </script>
  
  <style>
  /* Estilos específicos para o componente PessoaDetails.vue */
  </style>
  