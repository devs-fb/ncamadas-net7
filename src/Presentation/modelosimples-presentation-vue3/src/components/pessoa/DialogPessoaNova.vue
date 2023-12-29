<template>
    <Dialog v-model:visible="pessoaCreateDialog" :style="{ width: '600px' }" :header="'Nova Pessoa'" :modal="true" class="p-fluid">
      <div class="field">
        <label for="tipoPessoa">Tipo de Pessoa</label>
        <Dropdown v-model="dropdownCriarPessoaTipo" :options="dropdownCriarPessoaTipoItems" optionLabel="label" @change="handleChange" />
      </div>
  
      <br /><hr /><br />
      
      <div v-if="pessoa.tipo === 'F'" class="field">
        <label for="nomeSocial">Nome Social</label>
        <InputText
          id="nomeSocial"
          v-model="pessoa.pessoaFisica.nomeSocial"
          required="true"
          autofocus
          :class="{'p-invalid': submitted && !pessoa.pessoaFisica.nomeSocial}"
        />
        <small class="p-invalid" v-if="submitted && !pessoa.pessoaFisica.nomeSocial">
          Nome Social é obrigatório.
        </small>
      </div>
  
      <div v-if="pessoa.tipo === 'J'" class="field">
        <label for="razaoSocial">Razão Social</label>
        <InputText
          id="razaoSocial"
          v-model="pessoa.pessoaJuridica.razaoSocial"
          required="true"
          autofocus
          :class="{'p-invalid': submitted && !pessoa.pessoaJuridica.razaoSocial}"
        />
        <small class="p-invalid" v-if="submitted && !pessoa.pessoaJuridica.razaoSocial">
          Razão Social é obrigatória.
        </small>
      </div>
  
      <div class="field" v-if="pessoa.tipo === 'F'">
        <label for="dataNascimento">Data de Nascimento</label>
        <Calendar id="dataNascimento" v-model="pessoa.pessoaFisica.dataNascimento" :showIcon="true" :showButtonBar="true"></Calendar>
      </div>
  
      <div class="field" v-if="pessoa.tipo === 'F'">
        <label for="genero">Gênero</label>
        <InputText id="genero" v-model.trim="pessoa.pessoaFisica.genero" />
      </div>
  
      <div class="field" v-if="pessoa.tipo === 'J'">
        <label for="nomeFantasia">Nome Fantasia</label>
        <InputText id="nomeFantasia" v-model.trim="pessoa.pessoaJuridica.nomeFantasia" />
      </div>
  
      <div class="field" v-if="pessoa.tipo === 'J'">
        <label for="cnae">CNAE</label>
        <InputText id="cnae" v-model.trim="pessoa.pessoaJuridica.cnae" />
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
        pessoaCreateDialog: false,
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
    methods: {
      handleChange(opcaoSelecionada) {
        const selecionada = opcaoSelecionada.value;
        opcaoSelecionada.value.selected = true;
        this.pessoa.tipo = selecionada.value;
        this.label = opcaoSelecionada.value.label;
      },
      hidePessoaDialog() {
        this.pessoaCreateDialog = false;
        this.submitted = false;
      },
      savePessoa() {
        this.submitted = true;
  
        if (this.pessoa.id) {
			// Se a pessoa tem um ID, atualize-a
			this.pessoaService.editPessoa(this.pessoa)
			.then(() => {
				// Lógica após a atualização da pessoa
				this.hidePessoaDialog();
				this.loadPessoas();
				this.$toast.add({
				severity: 'success',
				summary: 'Successful',
				detail: 'Pessoa atualizada com sucesso',
				life: 3000,
				});
			})
			.catch((error) => {
				// Trate erros durante a atualização da pessoa
				console.error('Erro ao atualizar pessoa:', error);
			});
		} else {
			// Se a pessoa não tem um ID, crie uma nova
			this.pessoaService.createPessoa(this.pessoa)
			.then(() => {
				// Lógica após a criação da pessoa
				this.hidePessoaDialog();
				this.loadPessoas();
				this.$toast.add({
				severity: 'success',
				summary: 'Successful',
				detail: 'Nova pessoa criada com sucesso',
				life: 3000,
				});
			})
			.catch((error) => {
				// Trate erros durante a criação da pessoa
				console.error('Erro ao criar pessoa:', error);
			});
		}
      }
    }
  };
  </script>
  
  <style scoped lang="scss">
  </style>
  