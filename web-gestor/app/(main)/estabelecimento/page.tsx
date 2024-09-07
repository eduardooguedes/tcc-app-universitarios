'use client';
import { zodResolver } from "@hookform/resolvers/zod";
import { parseCookies } from "nookies";
import { Button } from 'primereact/button';
import { InputText } from "primereact/inputtext";
import { Toast } from 'primereact/toast';
import { Toolbar } from 'primereact/toolbar';
import { useEffect, useRef, useState } from 'react';
import { FormProvider, useForm } from 'react-hook-form';
import { z } from "zod";
import { api } from '../services/api';
import { InputMask } from "primereact/inputmask";
import { toastFail, toastSuccess } from "../services/toast";

const Estabelecimento = () => {

  const toast = useRef<Toast>(null);
  const [isLoading, setIsLoading] = useState(false);
  const [idEstabelecimento, setIdEstabelecimento] = useState('');
  const [idEndereco, setIdEndereco] = useState('');

  useEffect(() => {

    const { ['Universitario.estabelecimento']: estabelecimento } = parseCookies();

    api.get(`/v1/estabelecimento/${estabelecimento}`).then(res => {

      setIdEstabelecimento(estabelecimento)

      const data = res.data.data;

      setValue("nomeFantasia", data.nomeFantasia)
      setValue("razaoSocial", data.razaoSocial)
      setValue("telefone", data.telefone)

      setIdEndereco(data.enderecoRetirada.id)

      setValue2("cep", data.enderecoRetirada.cep)
      setValue2("logradouro", data.enderecoRetirada.logradouro)
      setValue2("numero", data.enderecoRetirada.numero.toString())
      setValue2("bairro", data.enderecoRetirada.bairro)
      setValue2("complemento", data.enderecoRetirada.complemento)
      setValue2("cidade", data.enderecoRetirada.cidade)
      setValue2("estado", data.enderecoRetirada.estado)

    }).catch(console.error);

  }, []);

  const createEstabelecimentoFormSchema = z.object({
    razaoSocial: z.string()
      .min(1, "Obrigatório informar razão social."),
    nomeFantasia: z.string()
      .min(1, "Obrigatório informar nome fantasia."),
    telefone: z.string()
      .min(1, "Obrigatório informar telefone.")
      .transform((value) => value.replace(/\D/g, "")),
  })

  const createEnderecoFormSchema = z.object({
    cep: z.string()
      .min(1, "Obrigatório informar o cep.")
      .transform((value) => value.replace(/\D/g, "")),
    numero: z.string()
      .min(1, "Obrigatório."),
    logradouro: z.string()
      .min(1, "Obrigatório informar o logradouro."),
    complemento: z.string(),
    bairro: z.string()
      .min(1, "Obrigatório informar o bairro."),
    estado: z.string()
      .min(1, "Obrigatório."),
    cidade: z.string()
      .min(1, "Obrigatório informar a cidade."),
  })

  type CreateEstabelecimentoFormData = z.infer<typeof createEstabelecimentoFormSchema>
  type CreateEnderecoFormData = z.infer<typeof createEnderecoFormSchema>

  const createEstabelecimentoForm = useForm<CreateEstabelecimentoFormData>({
    resolver: zodResolver(createEstabelecimentoFormSchema)
  });

  const createEnderecoForm = useForm<CreateEnderecoFormData>({
    resolver: zodResolver(createEnderecoFormSchema)
  });

  const {
    handleSubmit,
    setValue,
    formState: { errors },
    register
  } = createEstabelecimentoForm

  const {
    handleSubmit: handleSubmit2,
    setValue: setValue2,
    formState: { errors: errors2 },
    register: register2
  } = createEnderecoForm

  async function handleEditarEstabelecimento(data: CreateEstabelecimentoFormData) {

    setIsLoading(true)

    const body = {
      nomeFantasia: data.nomeFantasia,
      razaoSocial: data.razaoSocial,
      telefone: data.telefone
    }

    api.put(`/v1/estabelecimento/${idEstabelecimento}`, body)
      .then(res => {
        toastSuccess(toast, 'Dados salvos com sucesso');
      })
      .catch(err => {
        toastFail(toast, err?.response?.data?.erro ?? 'Falha ao comunicar com o servidor')
      })

    setIsLoading(false)
  }

  async function handleEditarEndereco(data: CreateEnderecoFormData) {
    setIsLoading(true)

    const body = {
      cep: data.cep,
      numero: data.numero,
      logradouro: data.logradouro,
      complemento: data.complemento,
      bairro: data.bairro,
      estado: data.estado,
      cidade: data.cidade
    }

    api.put(`/v1/estabelecimento/${idEstabelecimento}/enderecoRetirada/${idEndereco}`, body)
      .then(res => {
        toastSuccess(toast, 'Dados salvos com sucesso');
      })
      .catch(err => {
        toastFail(toast, err?.response?.data?.erro ?? 'Falha ao comunicar com o servidor');
      })

    setIsLoading(false)
  }

  const buttonEstabelecimentoTemplate = () => {
    return (
      <div className="my-2">
        <Button label="Salvar" icon="pi pi-save" severity="secondary" className=" mr-2" loading={isLoading} onClick={handleSubmit(handleEditarEstabelecimento)} />
      </div>
    );
  };

  const buttonEnderecoTemplate = () => {
    return (
      <div className="my-2">
        <Button label="Salvar" icon="pi pi-save" severity="secondary" className=" mr-2" loading={isLoading} onClick={handleSubmit2(handleEditarEndereco)} />
      </div>
    );
  };

  return (
    <div className="grid crud-demo">
      <div className="col-12">
        <div className="card">

          <div className="flex flex-column md:flex-row md:justify-content-between md:align-items-center">
            <h5 className="m-0">Estabelecimento</h5>
          </div>

          <FormProvider {...createEstabelecimentoForm}>
            <form onSubmit={handleSubmit(handleEditarEstabelecimento)} className="py-4">
              <Toast ref={toast} />

              <div className="grid formgrid">
                <div className="col-12 mb-2">
                  <div className="flex flex-column gap-2">
                    <label htmlFor="username">Razão social</label>
                    <InputText {...register('razaoSocial')} className={errors.razaoSocial?.message ? 'p-invalid' : ''} />
                    {errors.razaoSocial?.message ? <small className="p-error">{errors.razaoSocial.message}</small> : <small className="p-error"></small>}
                  </div>
                </div>
              </div>

              <div className="grid formgrid">
                <div className="col-12 mb-2">
                  <div className="flex flex-column gap-2">
                    <label htmlFor="username">Nome fantasia</label>
                    <InputText {...register('nomeFantasia')} className={errors.nomeFantasia?.message ? 'p-invalid' : ''} />
                    {errors.nomeFantasia?.message ? <small className="p-error">{errors.nomeFantasia.message}</small> : <small className="p-error"></small>}
                  </div>
                </div>
              </div>

              <div className="grid formgrid">
                <div className="col-12 mb-2 lg:col-4">
                  <div className="flex flex-column gap-2">
                    <label htmlFor="username">Telefone</label>
                    <InputMask {...register('telefone')} className={(errors.telefone?.message ? 'p-invalid' : '')} mask="(99) 99999-9999" />
                    {errors.telefone?.message ? <small className="p-error">{errors.telefone.message}</small> : <small className="p-error"></small>}
                  </div>
                </div>
              </div>

              <Toolbar className="mb-4" center={buttonEstabelecimentoTemplate}></Toolbar>
            </form>
          </FormProvider>

          <FormProvider {...createEnderecoForm}>
            <form onSubmit={handleSubmit2(handleEditarEndereco)} className="py-4">
              <Toast ref={toast} />

              <div className="grid formgrid">
                <div className="col-12 mb-2 lg:col-2">
                  <div className="flex flex-column gap-2">
                    <label htmlFor="username">CEP</label>
                    <InputText {...register2('cep')} className={errors2.cep?.message ? 'p-invalid' : ''} />
                    {errors2.cep?.message ? <small className="p-error">{errors2.cep.message}</small> : <small className="p-error"></small>}
                  </div>
                </div>
              </div>

              <div className="grid formgrid">
                <div className="col-12 mb-2 lg:col-10">
                  <div className="flex flex-column gap-2">
                    <label htmlFor="username">Logradouro</label>
                    <InputText {...register2('logradouro')} className={errors2.logradouro?.message ? 'p-invalid' : ''} />
                    {errors2.logradouro?.message ? <small className="p-error">{errors2.logradouro.message}</small> : <small className="p-error"></small>}
                  </div>
                </div>

                <div className="col-12 mb-2 lg:col-2">
                  <div className="flex flex-column gap-2">
                    <label htmlFor="username">Nº</label>
                    <InputText {...register2('numero')} className={errors2.numero?.message ? 'p-invalid' : ''} />
                    {errors2.numero?.message ? <small className="p-error">{errors2.numero.message}</small> : <small className="p-error"></small>}
                  </div>
                </div>
              </div>

              <div className="grid formgrid">
                <div className="col-12 mb-2 lg:col-6">
                  <div className="flex flex-column gap-2">
                    <label htmlFor="username">Complemento</label>
                    <InputText {...register2('complemento')} className={errors2.complemento?.message ? 'p-invalid' : ''} />
                    {errors2.complemento?.message ? <small className="p-error">{errors2.complemento.message}</small> : <small className="p-error"></small>}
                  </div>
                </div>

                <div className="col-12 mb-2 lg:col-6">
                  <div className="flex flex-column gap-2">
                    <label htmlFor="username">Bairro</label>
                    <InputText {...register2('bairro')} className={errors2.bairro?.message ? 'p-invalid' : ''} />
                    {errors2.bairro?.message ? <small className="p-error">{errors2.bairro.message}</small> : <small className="p-error"></small>}
                  </div>
                </div>
              </div>

              <div className="grid formgrid">
                <div className="col-12 mb-2 lg:col-2">
                  <div className="flex flex-column gap-2">
                    <label htmlFor="username">UF</label>
                    <InputText {...register2('estado')} className={errors2.estado?.message ? 'p-invalid' : ''} />
                    {errors2.estado?.message ? <small className="p-error">{errors2.estado.message}</small> : <small className="p-error"></small>}
                  </div>
                </div>

                <div className="col-12 mb-2 lg:col-10">
                  <div className="flex flex-column gap-2">
                    <label htmlFor="username">Cidade</label>
                    <InputText {...register2('cidade')} className={errors2.cidade?.message ? 'p-invalid' : ''} />
                    {errors2.cidade?.message ? <small className="p-error">{errors2.cidade.message}</small> : <small className="p-error"></small>}
                  </div>
                </div>
              </div>

              <Toolbar className="mb-4" center={buttonEnderecoTemplate}></Toolbar>
            </form>
          </FormProvider>
        </div>
      </div>
    </div>
  );
};

export default Estabelecimento;
