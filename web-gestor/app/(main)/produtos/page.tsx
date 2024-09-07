'use client';
import { zodResolver } from '@hookform/resolvers/zod';
import { Button } from 'primereact/button';
import { Column } from 'primereact/column';
import { DataTable } from 'primereact/datatable';
import { Dialog } from 'primereact/dialog';
import { Dropdown } from 'primereact/dropdown';
import { FileUpload } from 'primereact/fileupload';
import { InputNumber } from 'primereact/inputnumber';
import { InputText } from 'primereact/inputtext';
import { InputTextarea } from 'primereact/inputtextarea';
import { Toast } from 'primereact/toast';
import { Toolbar } from 'primereact/toolbar';
import { useEffect, useRef, useState } from 'react';
import { Controller, FormProvider, useForm } from 'react-hook-form';
import { z } from 'zod';
import { ICategoria } from '../models/ICategoria';
import { IProduto } from '../models/IProduto';
import { ITipoProduto } from '../models/ITipoProduto';
import { api } from '../services/api';
import { toastFail, toastSuccess } from '../services/toast';

export const Produtos = () => {

  const [isLoading, setIsLoading] = useState(false);
  const [produtos, setProdutos] = useState<IProduto[]>([]);
  const [produtosFiltrados, setPedidosFiltrados] = useState<IProduto[]>([]);
  const [categorias, setCategorias] = useState<string[]>();
  const [categoria, setCategoria] = useState<string>();
  const [produtoDialog, setProdutoDialog] = useState(false);
  const [deleteProdutoDialog, setDeleteProdutoDialog] = useState(false);
  const [produto, setProduto] = useState<IProduto | null>();
  const [listaCategoria, setListaCategoria] = useState<IOptionItem[] | undefined>(undefined);
  const [listaTipoProduto, setListaTipoProduto] = useState<IOptionItem[] | undefined>(undefined);
  const [imagem, setImagem] = useState<File>();
  const [imagemNome, setImagemNome] = useState<string>();


  const popRetirar = useRef(null);
  const toast = useRef<Toast>(null);
  const dt = useRef<DataTable<any>>(null);
  const fileUploadRef = useRef(null);

  useEffect(() => {
    api.get('/v1/estabelecimento/produto/lista').then((res) => {
      const _produtos = res.data.data;

      updateProdutos(_produtos);
    });
  }, []);

  const updateProdutos = (produtos: IProduto[]) => {
    setProdutos(produtos);
    setPedidosFiltrados(produtos);

    const unique = produtos.map((x: any) => x.categoria.descricao.toUpperCase()).filter((x: any, index: number, array: any) => array.indexOf(x) === index);

    setCategorias(unique);
    setCategoria('');
  }

  useEffect(() => {
    const fetchData = async () => {
      const res = await api.get('/v1/estabelecimento/configuracoesDeProduto');

      const data = res.data.data;

      setListaCategoria(data.categoriasDeProduto.map((x: ICategoria) => ({ value: x.id, option: x.descricao })))
      setListaTipoProduto(data.tiposDeProduto.map((x: ITipoProduto) => ({ value: x.id, option: x.descricao })))
    }

    fetchData()
      .catch(console.error);
  }, []);

  const filtrarCategoria = (categoriaFiltro: string) => {

    let clear = false;

    if (categoria == categoriaFiltro) {
      clear = true;
      setCategoria('')
    }
    else {
      setCategoria(categoriaFiltro)
    }

    setPedidosFiltrados(produtos.filter((x: any) => clear || x.categoria.descricao.toUpperCase() == categoriaFiltro));
  }

  const clearProduto = () => {
    setValue("idCategoria", -1);
    setValue("idTipoProduto", -1);
    setValue("nome", "");
    setValue("descricao", "");
    setValue("preco", 0);
    setValue("tempoEmMinutosParaRetirada", 0);
  };

  const openNew = () => {
    reset();
    setIsLoading(false)
    setProduto(null);
    clearProduto();
    setProdutoDialog(true);
  };

  const hideDialog = () => {
    setProdutoDialog(false);
  };

  const hideDeleteProdutoDialog = () => {
    setDeleteProdutoDialog(false);
  };

  const handleCadastrarProdutos = (data: CreateProdutoFormData) => {

    setIsLoading(true)


    const body = {
      idCategoria: data.idCategoria,
      idTipoProduto: data.idTipoProduto,
      nome: data.nome,
      descricao: data.descricao,
      preco: data.preco,
      tempoEmMinutosParaRetirada: data.tempoEmMinutosParaRetirada,
      adicionais: []
    }

    if (produto?.id) {
      api.put(`/v1/estabelecimento/produto/${produto?.id}`, body)
        .then(res => {
          toastSuccess(toast, 'Produto salvo com sucesso');

          const novo = res.data.data;

          if (imagem) {
            const formData = new FormData();
            formData.append("imagem", imagem)

            api.post(`/v1/estabelecimento/produto/${novo.id}/imagem`, formData)
              .then(res => {
                novo.imagem = res.data.data;
              })
          }

          let _produtos = [...(produtos as any)];

          const index = findIndexById(novo.id);

          _produtos[index] = novo;

          updateProdutos(_produtos);
          setProdutoDialog(false);
          setProduto(null);
        })
        .catch(err => {
          toastFail(toast, err?.response?.data?.erro ?? 'Falha ao comunicar com o servidor');
        })
        .finally(() => setIsLoading(false))
    } else {
      api.post('/v1/estabelecimento/produto', body)
        .then(res => {
          toastSuccess(toast, 'Produto salvo com sucesso');

          const novo = res.data.data;

          if (imagem) {
            const formData = new FormData();
            formData.append("imagem", imagem)

            api.post(`/v1/estabelecimento/produto/${novo.id}/imagem`, formData)
              .then(res => {
                novo.imagem = res.data.data;
              })
          }

          let _produtos = [...(produtos as any)];

          _produtos.push(novo);

          updateProdutos(_produtos);
          setProdutoDialog(false);
          setProduto(null);
        })
        .catch(err => {
          toastFail(toast, err?.response?.data?.erro ?? 'Falha ao comunicar com o servidor');
        })
        .finally(() => setIsLoading(false))
    }
  };

  const createProdutoFormSchema = z.object({
    idCategoria: z.number()
      .min(1, "Informe a categoria do produto."),
    idTipoProduto: z.number()
      .min(1, "Informe o tipo do produto."),
    nome: z.string()
      .min(1, "Informe um nome para o produto.")
      .max(25, "Informe um nome para o produto em até 25 caracteres."),
    descricao: z.string()
      .max(300, "Informe uma descrição para o produto em até 300 caracteres."),
    preco: z.number(),
    tempoEmMinutosParaRetirada: z.number()
  });

  type CreateProdutoFormData = z.infer<typeof createProdutoFormSchema>

  const createProdutoForm = useForm<CreateProdutoFormData>({
    resolver: zodResolver(createProdutoFormSchema)
  });

  const {
    control,
    handleSubmit,
    getValues,
    setValue,
    formState: { errors },
    register,
    reset
  } = createProdutoForm

  const editProduto = (produto: IProduto) => {
    setProduto({ ...produto });

    api.get(`/v1/estabelecimento/produto/${produto.id}`).then((res) => {

      const _produto = res.data.data as any;

      setProdutoDialog(true);

      setValue("idCategoria", _produto.categoria.id);
      setValue("idTipoProduto", _produto.tipo.id);
      setValue("nome", _produto.nome);
      setValue("descricao", _produto.descricao);
      setValue("preco", _produto.preco);
      setValue("tempoEmMinutosParaRetirada", _produto.tempoEmMinutosParaRetirada);
    });

  };

  const confirmDeleteProduto = (produto: IProduto) => {
    setProduto(produto);
    setDeleteProdutoDialog(true);
  };

  const deleteProduto = () => {

    api.delete(`/v1/estabelecimento/produtodefuncionamento/${produto?.id}`)
      .then(res => {
        const _produtos = (produtos as any)?.filter((val: any) => val.id !== produto?.id);
        setProdutos(_produtos);

        toastSuccess(toast, 'Horário excluído com sucesso');
      })
      .catch(err => {
        toastFail(toast, err?.response?.data?.erro ?? 'Falha ao comunicar com o servidor');
      })
      .finally(() => {
        setDeleteProdutoDialog(false);
        setProduto(null);
      });
  };

  const findIndexById = (id: string) => {
    let index = -1;
    for (let i = 0; i < (produtos as any)?.length; i++) {
      if ((produtos as any)[i].id === id) {
        index = i;
        break;
      }
    }

    return index;
  };

  const rightToolbarTemplate = () => {
    return (
      <div className="my-2">
        <Button label="Novo" icon="pi pi-plus" severity="secondary" className=" mr-2" onClick={openNew} />
      </div>
    );
  };

  const formatCurrency = (value: number) => {
    return value.toLocaleString('pt-BR', {
      style: 'currency',
      currency: 'BRL'
    });
  };

  const imageBodyTemplate = (row: IProduto) => {
    return <img height={60} src={row.imagem} onError={(e) => ((e.target as HTMLImageElement).src = '')} alt={row.imagem} className="shadow-2" width={100} />;
  };

  const priceTemplate = (row: IProduto) => {
    return <span className="text-right">{formatCurrency(row.preco)}</span>;
  };

  const nomeTemplate = (row: IProduto) => {
    return (
      <div className="flex flex-column">
        <span><strong>{row.nome}</strong></span>
        <span>{row.descricao}</span>
      </div>
    );
  };

  const statusTemplate = (row: IProduto) => {
    return <span className={`product-badge status-${row.situacao.descricao == 'Ativo' ? 'instock' : 'outofstock'}`}>{row.situacao.descricao}</span>;
  };

  const actionBodyTemplate = (rowData: IProduto) => {
    return (
      <>
        <Button icon="pi pi-pencil" rounded severity="info" className="mr-2" onClick={() => editProduto(rowData)} />
        {/* <Button icon="pi pi-trash" rounded severity="danger" onClick={() => confirmDeleteProduto(rowData)} /> */}
      </>
    );
  };

  const header = (
    <div className="flex flex-column md:flex-row md:justify-content-between md:align-items-center">
      <h5 className="m-0">Produtos</h5>
    </div>
  );

  const produtoDialogFooter = (
    <>
      <div className="flex justify-content-between">
        <Button label="Adicional" severity="info" icon="pi pi-plus" loading={isLoading} onClick={() => { }} />
        <div>
          <Button label="Cancelar" severity="secondary" icon="pi pi-times" text onClick={hideDialog} />
          <Button label="Salvar" severity="secondary" icon="pi pi-check" loading={isLoading} onClick={handleSubmit(handleCadastrarProdutos)} />
        </div>
      </div>
    </>
  );
  const deleteProdutoDialogFooter = (
    <>
      <Button label="Não" severity="secondary" icon="pi pi-times" text onClick={hideDeleteProdutoDialog} />
      <Button label="Sim" severity="secondary" icon="pi pi-check" onClick={deleteProduto} />
    </>
  );

  const itemTemplate = (file: any) => {
    return (
      <div className="flex justify-content-center align-items-center">
        <img alt={file.name} role="presentation" src={file.objectURL} width={100} />
      </div>
    );
  };

  const onTemplateSelect = (e: any) => {
    let files = e.files;

    Object.keys(files).forEach((key) => {
      setImagem(files[key]);
      setImagemNome(files[key].name)
    });
  };

  const emptyTemplate = () => {
    return (
      <div className="flex align-items-center flex-column">
        <i className="pi pi-image mt-3 p-1" style={{ fontSize: '2em', borderRadius: '50%', backgroundColor: 'var(--surface-b)', color: 'var(--surface-d)' }}></i>
        <span style={{ fontSize: '1.2em', color: 'var(--text-color-secondary)' }} className="my-2">
          Arraste uma imagem aqui
        </span>
      </div>
    );
  };

  const chooseOptions = { icon: 'pi pi-fw pi-images', label: 'Procurar', className: 'custom-choose-btn p-button-rounded p-button-outlined' };
  const uploadOptions = { icon: 'pi pi-fw pi-cloud-upload', iconOnly: true, className: 'd-none' };
  const cancelOptions = { icon: 'pi pi-fw pi-times', iconOnly: true, className: 'd-none' };

  return (
    <div className="grid crud-demo">
      <div className="col-12">
        <div className="card">
          <Toast ref={toast} />
          <Toolbar className="mb-4" right={rightToolbarTemplate}></Toolbar>

          {categorias?.length > 0 &&
            <div className="flex align-items-center justify-content-center gap-2 mb-4">
              {categorias.map((x: any) => <Button style={{ background: categoria == x ? "#ff3300" : "#bbb", borderColor: categoria == x ? "#ff3300" : "#bbb" }} key={x} onClick={() => filtrarCategoria(x)}>{x}</Button>)}
            </div>
          }

          <DataTable
            ref={dt}
            value={produtosFiltrados}
            dataKey="id"
            className="datatable-responsive"
            emptyMessage="Nenhum produto encontrado."
            header={header}
          >
            <Column style={{ width: '3em' }} />
            <Column header="Imagem" body={imageBodyTemplate} />
            <Column field="nome" header="Nome" body={nomeTemplate} />
            <Column field="tempoEmMinutosParaRetirada" header="Tempo p/ retirada" />
            <Column field="inventoryStatus" header="Situação" body={statusTemplate} />
            <Column className='text-right' align='right' field="preco" header="Preço" body={priceTemplate} />
            <Column className='text-right' body={actionBodyTemplate} headerStyle={{ minWidth: '10rem' }}></Column>
          </DataTable>

          <Dialog visible={produtoDialog} style={{ width: '850px' }} header="Cadastro" modal className="p-fluid" footer={produtoDialogFooter} onHide={hideDialog}>

            <FormProvider {...createProdutoForm}>
              <form onSubmit={handleSubmit(handleCadastrarProdutos)} className="py-4">

                <div className="field col text-center">
                  <FileUpload ref={fileUploadRef} name="demo[]" accept="image/*" maxFileSize={1000000} style={{ width: '300px', margin: "0 auto" }}
                    itemTemplate={itemTemplate} emptyTemplate={emptyTemplate} onSelect={onTemplateSelect}
                    chooseOptions={chooseOptions} uploadOptions={uploadOptions} cancelOptions={cancelOptions} />
                </div>

                <div className="formgrid grid">
                  <div className="field col">
                    <label>Nome</label>
                    <InputText {...register('nome')} className={(errors.nome?.message ? 'p-invalid' : '')} />
                    {errors.nome?.message ? <small className="p-error">{errors.nome.message}</small> : <small className="p-error"></small>}
                  </div>

                  <div className="field col">
                    <label>Tipo de produto</label>
                    <Controller name="idTipoProduto" control={control} render={({ field }) => (
                      <Dropdown id={field.name} value={field.value} onChange={(e) => field.onChange(e.value)} options={listaTipoProduto} optionLabel="option" optionValue="value"
                        placeholder="Tipo de Produto" className={(errors.idTipoProduto?.message ? 'p-invalid' : '')} />
                    )} />
                    {errors.idTipoProduto?.message ? <small className="p-error">{errors.idTipoProduto.message}</small> : <small className="p-error"></small>}
                  </div>

                  <div className="field col">
                    <label>Categoria</label>
                    <Controller name="idCategoria" control={control} render={({ field }) => (
                      <Dropdown id={field.name} value={field.value} onChange={(e) => field.onChange(e.value)} options={listaCategoria} optionLabel="option" optionValue="value"
                        placeholder="Categoria" className={(errors.idTipoProduto?.message ? 'p-invalid' : '')} />
                    )} />
                    {errors.idCategoria?.message ? <small className="p-error">{errors.idCategoria.message}</small> : <small className="p-error"></small>}
                  </div>
                </div>

                <div className="formgrid grid">
                  <div className="field col">
                    <label>Tempo de preparo (min)</label>
                    <InputNumber value={getValues().tempoEmMinutosParaRetirada} onValueChange={(e) => setValue('tempoEmMinutosParaRetirada', e.value ?? 0)} useGrouping={false} className={errors.quantidadeDePedidosDaPrimeiraRetirada?.message ? 'p-invalid' : ''} min={0} />
                    {errors.tempoEmMinutosParaRetirada?.message ? <small className="p-error">{errors.tempoEmMinutosParaRetirada.message}</small> : <small className="p-error"></small>}
                  </div>

                  <div className="field col">
                    <label htmlFor="price">Preço</label>
                    <InputNumber value={getValues().preco} onValueChange={(e) => setValue('preco', e.value ?? 0)} mode="currency" currency="BRL" locale="pt-br" />
                    {errors.preco?.message ? <small className="p-error">{errors.preco.message}</small> : <small className="p-error"></small>}
                  </div>
                </div>

                <div className="formgrid grid">
                  <div className="field col">
                    <label>Descrição</label>
                    <InputTextarea {...register('descricao')} required rows={3} cols={20} className={(errors.descricao?.message ? 'p-invalid' : '')} />
                    {errors.descricao?.message ? <small className="p-error">{errors.descricao.message}</small> : <small className="p-error"></small>}
                  </div>
                </div>
              </form>
            </FormProvider>
          </Dialog>

          <Dialog visible={deleteProdutoDialog} style={{ width: '450px' }} header="Confirmar exclusão" modal footer={deleteProdutoDialogFooter} onHide={hideDeleteProdutoDialog}>
            <div className="flex align-items-center justify-content-center">
              <i className="pi pi-exclamation-triangle mr-3" style={{ fontSize: '2rem' }} />
              {produto && (
                <span>
                  Deseja realmente excluir o produto selecionado?
                </span>
              )}
            </div>
          </Dialog>
        </div>
      </div>
    </div >
  );
};

export default Produtos;