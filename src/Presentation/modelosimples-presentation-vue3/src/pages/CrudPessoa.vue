<template>
	<div class="grid">
		<div class="col-12">
			<div class="card">
				<Toast/>
				<Toolbar class="mb-4">
					<template v-slot:start>
						<div class="my-2">
							<Button label="New" icon="pi pi-plus" class="p-button-success mr-2" @click="openNew" />
							<Button label="Delete" icon="pi pi-trash" class="p-button-danger" @click="confirmDeleteSelected" :disabled="!selectedPessoas || !selectedPessoas.length" />
						</div>
					</template>
					<template v-slot:end>
						<FileUpload mode="basic" accept="image/*" :maxFileSize="1000000" label="Import" chooseLabel="Import" class="mr-2 inline-block" />
						<Button label="Export" icon="pi pi-upload" class="p-button-help" @click="exportCSV($event)"  />
					</template>
				</Toolbar>

				<DataTable :value="pessoas" ref="dt" :selection="selectedPessoas" dataKey="id" :paginator="true" :rows="10" :rowsPerPageOptions="[5,10,25,50,100]" currentPageReportTemplate="Apresentando {first} até {last} de {totalRecords} pessoas" responsiveLayout="scroll">
					<template #header>
						<div class="flex flex-column md:flex-row md:justify-content-between md:align-items-center">
							<h5 class="m-0">Gerenciar Pessoas</h5>
							<span class="block mt-2 md:mt-0 p-input-icon-left">
								<i class="pi pi-search" />
								<InputText v-model="filters['global'].value" placeholder="Buscar..." />
							</span>
						</div>
					</template>
					<Column selectionMode="multiple" headerStyle="width: 3rem"></Column>
					<Column field="id" header="ID" headerStyle="width:14%; min-width:10rem;"></Column>
					<Column field="tipo" header="Tipo" :sortable="true" headerStyle="width:14%; min-width:10rem;"></Column>
					<Column field="nomeSocial" header="Nome" :sortable="true" headerStyle="width:14%; min-width:10rem;">
						<template #body="slotProps">
							<span class="p-column-title">Nome</span>
							{{ 
							slotProps.data.pessoaFisica ? slotProps.data.pessoaFisica.nomeSocial 
							: (slotProps.data.pessoaJuridica ? slotProps.data.pessoaJuridica.razaoSocial 
							: 'N/A') 
							}}
						</template>
					</Column>
					<Column field="nomeFantasia" header="Nome Fantasia" :sortable="true" headerStyle="width:14%; min-width:10rem;">
						<template #body="slotProps">
							<span class="p-column-title">Nome Fantasia</span>
							{{ 
							slotProps.data.pessoaJuridica ? slotProps.data.pessoaJuridica.nomeFantasia : 'N/A' 
							}}
						</template>						
					</Column>
					<Column field="dataNascimento" header="Data Nascimento" :sortable="true" headerStyle="width:14%; min-width:10rem;">
						<template #body="slotProps">
							<span class="p-column-title">Data Nascimento</span>
							{{ 
							slotProps.data.pessoaFisica ? slotProps.data.pessoaFisica.dataNascimento : 'N/A' 
							}}
						</template>					
					</Column>
					<Column field="genero" header="Gênero" :sortable="true" headerStyle="width:14%; min-width:10rem;">
						<template #body="slotProps">
							<span class="p-column-title">Gênero</span>
							{{ 
							slotProps.data.pessoaFisica ? slotProps.data.pessoaFisica.genero : 'N/A' 
							}}
						</template>	
					</Column>
					<Column field="cnae" header="CNAE" :sortable="true" headerStyle="width:14%; min-width:10rem;">
						<template #body="slotProps">
							<span class="p-column-title">CNAE</span>
							{{ 
							slotProps.data.pessoaJuridica ? slotProps.data.pessoaJuridica.cnae : 'N/A' 
							}}
						</template>						
					</Column>
					<Column headerStyle="min-width:10rem;">
						<template #body="slotProps">
							<Button icon="pi pi-pencil" class="p-button-rounded p-button-success mr-2" @click="editPessoa(slotProps.data)" />
							<Button icon="pi pi-trash" class="p-button-rounded p-button-warning mt-2" @click="confirmDeletePessoa(slotProps.data)" />
						</template>
					</Column>
				</DataTable>

				<DialogPessoaNova @hidePessoaDialog="hidePessoaDialog" @savePessoa="savePessoa" />
				<DialogPessoaEditar @hidePessoaDialog="hidePessoaDialog" @savePessoa="savePessoa" />

				<Dialog v-model:visible="deletePessoaDialog" :style="{width: '450px'}" header="Confirm" :modal="true">
					<div class="flex align-items-center justify-content-center">
						<i class="pi pi-exclamation-triangle mr-3" style="font-size: 2rem" />
						<span v-if="pessoa">Tem certeza que deseja remover <b>{{pessoa.name}}</b>?</span>
					</div>
					<template #footer>
						<Button label="No" icon="pi pi-times" class="p-button-text" @click="deletePessoaDialog = false"/>
						<Button label="Yes" icon="pi pi-check" class="p-button-text" @click="deletePessoa" />
					</template>
				</Dialog>

				<Dialog v-model:visible="deletePessoasDialog" :style="{width: '450px'}" header="Confirm" :modal="true">
					<div class="flex align-items-center justify-content-center">
						<i class="pi pi-exclamation-triangle mr-3" style="font-size: 2rem" />
						<span v-if="pessoa">Tem certeza que deseja remover as pessoas selecionadas?</span>
					</div>
					<template #footer>
						<Button label="No" icon="pi pi-times" class="p-button-text" @click="deletePessoasDialog = false"/>
						<Button label="Yes" icon="pi pi-check" class="p-button-text" @click="deleteSelectedPessoas" />
					</template>
				</Dialog>
			</div>
		</div>
	</div>

</template>

<script>
import { FilterMatchMode } from 'primevue/api';
import PessoaService from '../service/PessoaService.js';

export default {
  data() {
    return {
		pessoas: null,
		pessoaCreateDialog: false,
		pessoaEditDialog: false,
		deletePessoaDialog: false,
		deletePessoasDialog: false,
		pessoa: {
		tipo: 'F',
		pessoaFisica: {
			nome: '',
			dataNascimento: '',
			genero: ''
		},
		pessoaJuridica: {
			nomeFantasia: '',
			cnae: ''
		}
		},
		selectedPessoas: null,
		filters: {},
		submitted: false,
		isPessoaFisica: false,
		isPessoaJuridica: false,
		isPessoaEdit: false,
		dropdownCriarPessoaTipoItems: [
			{name: 'Pessoa Física', code: 'F', label: 'Pessoa Física', value: 'F'},
			{name: 'Pessoa Jurídica', code: 'J', label: 'Pessoa Jurídica', value: 'J'}
		],
		dropdownCriarPessoaTipo: 'F'
    };
  },
  pessoaService: null,
  created() {
    this.pessoaService = new PessoaService();
    this.initFilters();
    this.loadPessoas();
  },
  methods: {
    initFilters() {
      this.filters = {
        global: { value: null, matchMode: FilterMatchMode.CONTAINS },
      };
    },
    loadPessoas() {
      this.pessoaService.getPessoas().then((data) => {
        this.pessoas = data;
      });
    },
	handleChange(opcaoSelecionada) {
		const selecionada = opcaoSelecionada.value;
		opcaoSelecionada.value.selected = true;
		console.log('Opção selecionada:', selecionada);
		this.pessoa.tipo = selecionada.value;
		console.log('Pessoa:', this.pessoa);
		this.label = opcaoSelecionada.value.label;
	},
    openNew() {
		this.pessoa = {
			tipo: 'F',
			pessoaFisica: {
			nome: '',
			dataNascimento: '',
			genero: ''
			},
			pessoaJuridica: {
			nomeFantasia: '',
			cnae: ''
			}
		};
		this.isPessoaEdit = false;
		this.submitted = false;
		this.pessoaCreateDialog = true;
		this.pessoaEditDialog = false;
    },
    hidePessoaDialog() {
		this.pessoaCreateDialog = false;
		this.pessoaEditDialog = false;
		this.submitted = false;
    },
	savePessoa() {
		this.submitted = true;
	},
    editPessoa(pessoa) {
		console.log(pessoa);
		this.isPessoaFisica = (pessoa.tipo === 'F');
		this.isPessoaJuridica = (pessoa.tipo === 'J');
		this.isPessoaEdit = true;
		this.pessoa = pessoa;
		this.pessoaCreateDialog = false;
		this.pessoaEditDialog = true;
    },
    confirmDeletePessoa(pessoa) {
      this.pessoa = pessoa;
      this.deletePessoaDialog = true;
    },
    deletePessoa() {
      // Lógica para excluir uma pessoa
    },
    confirmDeleteSelected() {
      this.deletePessoasDialog = true;
    },
    deleteSelectedPessoas() {
      // Lógica para excluir várias pessoas selecionadas
    },
  },
};
</script>

<style scoped lang="scss">
@import '../assets/demo/badges.scss';
</style>
