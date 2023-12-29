<template>
    <Dialog v-model:visible="pessoaEditDialog" :style="{width: '600px'}" :header="`${isPessoaEdit ? 'Editar Pessoa ' + isPessoaFisica ? 'Física' : 'Jurídica' : 'Nova Pessoa'}`" :modal="true" class="p-fluid">
        <div class="field">
            <label for="nomeSocial" v-if="isPessoaFisica">Nome Social</label>
            <label for="razaoSocial" v-if="isPessoaJuridica">Razão Social</label>
            <InputText id="nomeSocial"
                :value="isPessoaFisica ? pessoa.pessoaFisica.nomeSocial : pessoa.pessoaJuridica.razaoSocial"
                required="true"
                autofocus
                :class="{'p-invalid': submitted && (!isPessoaFisica ? !pessoa.pessoaJuridica.razaoSocial : !pessoa.pessoaFisica.nomeSocial)}" />
            <small class="p-invalid" v-if="submitted && (!isPessoaFisica ? !pessoa.pessoaJuridica.razaoSocial : !pessoa.pessoaFisica.nomeSocial)">
                {{ isPessoaFisica ? 'Nome Social' : 'Razão Social' }} is required.
            </small>
        </div>

        <div class="field">
            <label for="dataNascimento" v-if="isPessoaFisica">Data de Nascimento</label>
            <InputText id="dataNascimento" v-model="pessoa.pessoaFisica.dataNascimento" v-if="isPessoaFisica" />
        </div>

        <div class="field">
            <label for="genero" v-if="isPessoaFisica">Gênero</label>
            <InputText id="genero" v-model.trim="pessoa.pessoaFisica.genero" v-if="isPessoaFisica" />
        </div>

        <div class="field">
            <label for="nomeFantasia" v-if="isPessoaJuridica">Nome Fantasia</label>
            <InputText id="nomeFantasia" v-model.trim="pessoa.pessoaJuridica.nomeFantasia" v-if="isPessoaJuridica" />
        </div>

        <div class="field">
            <label for="cnae" v-if="isPessoaJuridica">CNAE</label>
            <InputText id="cnae" v-model.trim="pessoa.pessoaJuridica.cnae" v-if="isPessoaJuridica" />
        </div>

        <template #footer>
            <Button label="Cancelar" icon="pi pi-times" class="p-button-text" @click="hidePessoaDialog"/>
            <Button label="Salvar" icon="pi pi-check" class="p-button-text" @click="savePessoa" />
        </template>
    </Dialog>
</template>

<script>
  export default {
    data() {
      return {
        pessoaEditDialog: false,
        pessoa: {
          tipo: 'F',
          pessoaFisica: {
            nomeSocial: '',
            dataNascimento: '',
            genero: ''
          },
          pessoaJuridica: {
            razaoSocial: '',
            nomeFantasia: '',
            cnae: ''
          }
        },
        isPessoaFisica: false,
        isPessoaJuridica: false,
        isPessoaEdit: false,
        submitted: false,
        dropdownCriarPessoaTipoItems: [
          { name: 'Pessoa Física', code: 'F', label: 'Pessoa Física', value: 'F' },
          { name: 'Pessoa Jurídica', code: 'J', label: 'Pessoa Jurídica', value: 'J' }
        ],
        dropdownCriarPessoaTipo: 'F'
      };
    },
    pessoaService: null,
}
</script>

<style scoped lang="scss">
</style>