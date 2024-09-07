'use client';
import { zodResolver } from "@hookform/resolvers/zod";
import { Button } from 'primereact/button';
import { InputMask } from "primereact/inputmask";
import { InputText } from "primereact/inputtext";
import { Toast } from 'primereact/toast';
import { Toolbar } from 'primereact/toolbar';
import { useEffect, useRef, useState } from 'react';
import { FormProvider, useForm } from 'react-hook-form';
import { z } from "zod";
import { api } from '../services/api';
import { dateToString, isValidDate } from "../services/date";
import { toastFail, toastSuccess } from "../services/toast";

const Gestor = () => {

  const toast = useRef<Toast>(null);
  const [isLoading, setIsLoading] = useState(false);

  useEffect(() => {

    api.get(`/v1/gestor/logado`).then(res => {

      const gestor = res.data.data;

      setValue("nome", gestor.nome)
      setValue("sobrenome", gestor.sobrenome)
      setValue("dataNascimento", dateToString(gestor.dataDeNascimento))
    }).catch(console.error);

  }, []);

  const createGestorFormSchema = z.object({
    nome: z.string()
      .min(1, "Obrigatório informar nome."),
    sobrenome: z.string()
      .min(1, "Obrigatório informar sobrenome."),
    dataNascimento: z.string()
      .refine((value) => isValidDate(value), {
        message: "Data inválida.",
      })
  });

  type createGestorForm = z.infer<typeof createGestorFormSchema>

  const createGestorForm = useForm<createGestorForm>({
    resolver: zodResolver(createGestorFormSchema)
  });

  const {
    handleSubmit,
    setValue,
    formState: { errors },
    register
  } = createGestorForm

  async function handleCadastrarGestor(data: createGestorForm) {

    setIsLoading(true)

    const body = {
      nome: data.nome,
      sobrenome: data.sobrenome,
      dataNascimento: new Date(data.dataNascimento).toISOString().split('T')[0]
    }

    api.put('/v1/gestor', body)
      .then(res => {
        toastSuccess(toast, 'Gestor atualizado com sucesso');
      })
      .catch(err => {
        toastFail(toast, err?.response?.data?.erro ?? 'Falha ao comunicar com o servidor');
      })

    setIsLoading(false)
  }

  const buttonTemplate = () => {
    return (
      <div className="my-2">
        <Button label="Salvar" icon="pi pi-save" severity="secondary" className=" mr-2" loading={isLoading} onClick={handleSubmit(handleCadastrarGestor)} />
      </div>
    );
  };

  return (
    <div className="grid crud-demo">
      <div className="col-12">
        <div className="card">

          <div className="flex flex-column md:flex-row md:justify-content-between md:align-items-center">
            <h5 className="m-0">Gestor</h5>
          </div>

          <FormProvider {...createGestorForm}>
            <form onSubmit={handleSubmit(handleCadastrarGestor)} className="py-4">
              <Toast ref={toast} />

              <div className="grid formgrid">
                <div className="col-12 mb-2 lg:col-4">
                  <div className="flex flex-column gap-2">
                    <label htmlFor="username">Nome</label>
                    <InputText {...register('nome')} className={errors.nome?.message ? 'p-invalid' : ''} />
                    {errors.nome?.message ? <small className="p-error">{errors.nome.message}</small> : <small className="p-error"></small>}
                  </div>
                </div>
              </div>

              <div className="grid formgrid">
                <div className="col-12 mb-2 lg:col-4">
                  <div className="flex flex-column gap-2">
                    <label htmlFor="username">Sobrenome</label>
                    <InputText {...register('sobrenome')} className={errors.sobrenome?.message ? 'p-invalid' : ''} />
                    {errors.sobrenome?.message ? <small className="p-error">{errors.sobrenome.message}</small> : <small className="p-error"></small>}
                  </div>
                </div>
              </div>

              <div className="grid formgrid">
                <div className="col-12 mb-2 lg:col-4">
                  <div className="flex flex-column gap-2">
                    <label htmlFor="username">Data Nascimento</label>
                    <InputMask {...register('dataNascimento')} className={(errors.dataNascimento?.message ? 'p-invalid' : '')} mask="99/99/9999" />
                    {errors.dataNascimento?.message ? <small className="p-error">{errors.dataNascimento.message}</small> : <small className="p-error"></small>}
                  </div>
                </div>
              </div>

              <Toolbar className="mb-4" center={buttonTemplate}></Toolbar>
            </form>
          </FormProvider>
        </div>
      </div>
    </div>
  );
};

export default Gestor;
