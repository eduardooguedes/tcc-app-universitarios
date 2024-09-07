'use client';
import { zodResolver } from '@hookform/resolvers/zod';
import { useRouter } from 'next/navigation';
import { setCookie } from 'nookies';
import { Button } from 'primereact/button';
import { InputMask } from 'primereact/inputmask';
import { InputText } from 'primereact/inputtext';
import { Toast } from 'primereact/toast';
import { classNames } from 'primereact/utils';
import { useEffect, useRef, useState } from 'react';
import { Controller, useForm } from 'react-hook-form';
import { z } from 'zod';
import { api } from '../../(main)/services/api';
import { toastFail, toastSuccess } from '../../(main)/services/toast';
import { buscaCEP, getEstados } from '../../(main)/services/utils';
import { Dropdown } from 'primereact/dropdown';
import { validarUsuario } from '../../(main)/services/security';

const CadastroEstabelecimento = () => {

  const router = useRouter();
  const [isLoading, setIsLoading] = useState(false);
  const toast = useRef<Toast>(null);
  const [estados, setEstados] = useState<IOptionItem[] | undefined>(undefined);

  useEffect(() => {

    const res = validarUsuario(null);

    if (res) {
      router.push(res);
    } else {
      setIsLoading(false)
    }
  }, []);

  const createEstabelecimentoFormSchema = z.object({
    razaoSocial: z.string()
      .min(1, "Obrigatório informar razão social."),
    nomeFantasia: z.string()
      .min(1, "Obrigatório informar nome fantasia."),
    cnpj: z.string()
      .min(1, "Obrigatório informar CNPJ.")
      .transform((value) => value.replace(/\D/g, "")),
    email: z.string()
      .min(1, "Obrigatório informar e-mail.")
      .email('E-mail inválido.')
      .toLowerCase(),
    telefone: z.string()
      .min(1, "Obrigatório informar telefone.")
      .transform((value) => value.replace(/\D/g, "")),
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

  type CreateEstabelecimentoFormSchema = z.infer<typeof createEstabelecimentoFormSchema>

  const createEstabelecimentoForm = useForm<CreateEstabelecimentoFormSchema>({
    resolver: zodResolver(createEstabelecimentoFormSchema)
  });

  const {
    control,
    handleSubmit,
    register,
    setValue,
    formState: { errors }
  } = createEstabelecimentoForm

  async function handleCadastrarEstabelecimento(data: CreateEstabelecimentoFormSchema) {

    setIsLoading(true)

    const body = {
      razaoSocial: data.razaoSocial,
      nomeFantasia: data.nomeFantasia,
      cnpj: data.cnpj,
      email: data.email,
      telefone: data.telefone,
      enderecoRetirada: {
        cep: data.cep,
        numero: data.numero,
        logradouro: data.logradouro,
        complemento: data.complemento,
        bairro: data.bairro,
        estado: data.estado,
        cidade: data.cidade
      }
    }

    api.post('/v1/estabelecimento', body)
      .then(res => {
        toastSuccess(toast, 'Estabelecimento cadastrado com sucesso');

        const response = res.data;
        const data = response.data;

        const expireTime = 60 * 60 * 8;
        setCookie(undefined, 'Universitario.estabelecimento', JSON.stringify(data), {
          maxAge: expireTime,
        })

        router.push('/estabelecimento_logo');
      })
      .catch(err => {
        toastFail(toast, err?.response?.data?.erro ?? 'Falha ao comunicar com o servidor')
      })
      .finally(() => setIsLoading(false))
  }

  useEffect(() => {
    setEstados(getEstados().map(x => ({ value: x.uf, option: x.uf })));
  }, []);

  const buscaEndereco = async (cep: string) => {
    cep = cep.replace("_", "")

    if (cep.length == 9) {

      const endereco = await buscaCEP(cep);

      setValue('bairro', endereco.bairro);
      setValue('logradouro', endereco.logradouro);
      setValue('cidade', endereco.localidade);
      setValue('estado', endereco.uf);
    }
  }

  const containerClassName = classNames('surface-ground flex align-items-center justify-content-center min-h-screen min-w-screen overflow-hidden');

  return (<>
    <Toast ref={toast} />
    <div className={containerClassName}>
      <div className="flex flex-column align-items-center justify-content-center">
        <div
          style={{
            borderRadius: '56px',
            padding: '0.3rem',
          }}
        >
          <div className="w-full surface-card py-4 px-5 sm:px-8" style={{ borderRadius: '53px' }}>
            <div className="text-center mb-1">
              <img src="/layout/images/Universitario_logo_laranja.png" alt="Image" height="100" className="mb-3" />
            </div>

            <h4>Cadastre seu estabelecimento</h4>
            <div>
              <div className="flex flex-column gap-1 mb-1">
                <label className="block text-600 font-medium mb-1">
                  Razão Social
                </label>
                <InputText {...register('razaoSocial')} placeholder="Nome" className={(errors.razaoSocial?.message ? 'p-invalid' : '')} style={{ padding: '1rem' }} />
                {errors.razaoSocial?.message ? <small className="p-error">{errors.razaoSocial.message}</small> : <small className="p-error"></small>}
              </div>

              <div className="flex flex-column gap-1 mb-1">
                <label className="block text-600 font-medium mb-1">
                  Nome fantasia
                </label>
                <InputText {...register('nomeFantasia')} placeholder="Nome Fantasia" className={(errors.nomeFantasia?.message ? 'p-invalid' : '')} style={{ padding: '1rem' }} />
                {errors.nomeFantasia?.message ? <small className="p-error">{errors.nomeFantasia.message}</small> : <small className="p-error"></small>}
              </div>

              <div className="flex flex-column gap-1 mb-1">
                <label className="block text-600 font-medium mb-1">
                  CNPJ
                </label>
                <InputMask {...register('cnpj')} placeholder="CNPJ" className={(errors.cnpj?.message ? 'p-invalid' : '')} style={{ padding: '1rem' }} mask='99.999.999/9999-99' />
                {errors.cnpj?.message ? <small className="p-error">{errors.cnpj.message}</small> : <small className="p-error"></small>}
              </div>

              <div className="flex flex-column gap-1 mb-1">
                <label className="block text-600 font-medium mb-1">
                  E-mail
                </label>
                <InputText {...register('email')} placeholder="E-mail" className={(errors.email?.message ? 'p-invalid' : '')} style={{ padding: '1rem' }} />
                {errors.email?.message ? <small className="p-error">{errors.email.message}</small> : <small className="p-error"></small>}
              </div>

              <div className="flex flex-column gap-1 mb-1">
                <label className="block text-600 font-medium mb-1">
                  Telefone
                </label>
                <InputMask {...register('telefone')} placeholder="Telefone" className={(errors.telefone?.message ? 'p-invalid' : '')} style={{ padding: '1rem' }} mask='(99) 99999-9999' />
                {errors.telefone?.message ? <small className="p-error">{errors.telefone.message}</small> : <small className="p-error"></small>}
              </div>

              <h4>Onde ele se encontra?</h4>

              <div className="grid formgrid">
                <div className="col-12 mb-2 lg:col-9">
                  <div className="flex flex-column gap-1 mb-1">
                    <label className="block text-600 font-medium mb-1">
                      Cep
                    </label>
                    <InputMask {...register('cep')} placeholder="Cep" className={(errors.cep?.message ? 'p-invalid' : '')} style={{ padding: '1rem' }} onChange={(e) => buscaEndereco(e.target.value)} mask="99999-999" />
                    {errors.cep?.message ? <small className="p-error">{errors.cep.message}</small> : <small className="p-error"></small>}
                  </div>
                </div>

                <div className="col-12 mb-2 lg:col-3">
                  <div className="flex flex-column gap-1 mb-1">
                    <label className="block text-600 font-medium mb-1">
                      Nº
                    </label>
                    <InputText {...register('numero')} placeholder="Número" className={(errors.numero?.message ? 'p-invalid' : '')} style={{ padding: '1rem' }} />
                    {errors.numero?.message ? <small className="p-error">{errors.numero.message}</small> : <small className="p-error"></small>}
                  </div>
                </div>
              </div>

              <div className="flex flex-column gap-1 mb-1">
                <label className="block text-600 font-medium mb-1">
                  Logradouro
                </label>
                <InputText {...register('logradouro')} placeholder="Logradouro" className={(errors.logradouro?.message ? 'p-invalid' : '')} style={{ padding: '1rem' }} />
                {errors.logradouro?.message ? <small className="p-error">{errors.logradouro.message}</small> : <small className="p-error"></small>}
              </div>

              <div className="flex flex-column gap-1 mb-1">
                <label className="block text-600 font-medium mb-1">
                  Complemento
                </label>
                <InputText {...register('complemento')} placeholder="Complemento" className={(errors.complemento?.message ? 'p-invalid' : '')} style={{ padding: '1rem' }} />
                {errors.complemento?.message ? <small className="p-error">{errors.complemento.message}</small> : <small className="p-error"></small>}
              </div>

              <div className="flex flex-column gap-1 mb-1">
                <label className="block text-600 font-medium mb-1">
                  Bairro
                </label>
                <InputText {...register('bairro')} placeholder="Bairro" className={(errors.bairro?.message ? 'p-invalid' : '')} style={{ padding: '1rem' }} />
                {errors.bairro?.message ? <small className="p-error">{errors.bairro.message}</small> : <small className="p-error"></small>}
              </div>

              <div className="grid formgrid">
                <div className="col-12 mb-2 lg:col-3">
                  <div className="flex flex-column gap-1 mb-1">
                    <label className="block text-600 font-medium mb-1">
                      UF
                    </label>
                    <Controller name="estado" control={control} render={({ field }) => (
                      <Dropdown id={field.name} value={field.value} onChange={(e) => field.onChange(e.value)} options={estados} optionLabel="option" optionValue="value"
                        placeholder="UF" className={(errors.estado?.message ? 'p-invalid' : '')} />
                    )} />
                    {errors.estado?.message ? <small className="p-error">{errors.estado.message}</small> : <small className="p-error"></small>}
                  </div>
                </div>

                <div className="col-12 mb-2 lg:col-9">
                  <div className="flex flex-column gap-1 mb-1">
                    <label className="block text-600 font-medium mb-1">
                      Cidade
                    </label>
                    <InputText {...register('cidade')} placeholder="Cidade" className={(errors.cidade?.message ? 'p-invalid' : '')} style={{ padding: '1rem' }} />
                    {errors.cidade?.message ? <small className="p-error">{errors.cidade.message}</small> : <small className="p-error"></small>}
                  </div>
                </div>
              </div>

              <Button label="Avançar" severity="secondary" className="w-full p-3 text-xl" loading={isLoading} onClick={handleSubmit(handleCadastrarEstabelecimento)}></Button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </>)
};

export default CadastroEstabelecimento;
