'use client';
import { zodResolver } from '@hookform/resolvers/zod';
import Link from 'next/link';
import { useRouter } from 'next/navigation';
import { setCookie } from 'nookies';
import { Button } from 'primereact/button';
import { InputText } from 'primereact/inputtext';
import { Password } from 'primereact/password';
import { Toast } from 'primereact/toast';
import { useRef, useState } from 'react';
import { useForm } from 'react-hook-form';
import { z } from 'zod';
import { api } from '../../(main)/services/api';
import { isValidDate, toDate } from '../../(main)/services/date';
import { classNames } from 'primereact/utils';
import { InputMask } from 'primereact/inputmask';
import { toastFail, toastSuccess } from '../../(main)/services/toast';

const CadastroGestor = () => {

  const router = useRouter();
  const [isLoading, setIsLoading] = useState(false);
  const toast = useRef<Toast>(null);

  const createGestorFormSchema = z.object({
    nome: z.string()
      .min(1, "Obrigatório informar nome."),
    sobrenome: z.string()
      .min(1, "Obrigatório informar sobrenome."),
    cpf: z.string()
      .min(1, "Obrigatório informar CPF.")
      .transform((value) => value.replace(/\D/g, "")),
    email: z.string()
      .min(1, "Obrigatório informar e-mail.")
      .email('E-mail inválido.')
      .toLowerCase(),
    dataNascimento: z.string()
      .min(1, "Obrigatório informar data de nascimento.")
      .refine((value) => isValidDate(value), {
        message: "Data inválida.",
      })
      .transform((value) => toDate(value)),
    senha: z.string()
      .min(1, "Obrigatório informar senha.")
      .min(8, "Informe uma senha que possua no mínimo 8 caracteres."),
    confirmarSenha: z.string()
      .min(1, "Obrigatório informar a confirmação de senha."),
  })
    .refine((value) => value.senha === value.confirmarSenha, {
      message: "Confirmação de senha não confere.",
      path: ["confirmarSenha"],
    });

  type CreateGestorFormData = z.infer<typeof createGestorFormSchema>

  const createGestorForm = useForm<CreateGestorFormData>({
    resolver: zodResolver(createGestorFormSchema)
  });

  const {
    handleSubmit,
    register,
    setValue,
    formState: { errors }
  } = createGestorForm

  async function handleCadastrarGestor(data: CreateGestorFormData) {

    setIsLoading(true);

    const body = {
      nome: data.nome,
      sobrenome: data.sobrenome,
      dataNascimento: data.dataNascimento.toISOString().substring(0, 10),
      cpf: data.cpf,
      email: data.email,
      senha: data.senha,
    }

    await api.post('/v1/gestor', body)
      .then(res => {
        console.log(123)
        toastSuccess(toast, 'Gestor cadastrado com sucesso');

        const response = res.data;
        const data = response.data;

        const { gestor: user, token } = data;

        const expireTime = 60 * 60 * 8;
        setCookie(undefined, 'Universitario.token', token, {
          maxAge: expireTime,
        })

        setCookie(undefined, 'Universitario.user', JSON.stringify(user), {
          maxAge: expireTime,
        })

        router.push('/cadastro_estabelecimento');
      })
      .catch(err => {
        toastFail(toast, err?.response?.data?.erro ?? 'Falha ao comunicar com o servidor')
      })
      .finally(() => setIsLoading(false))
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

            <div>
              <div className="flex flex-column gap-1 mb-1">
                <label className="block text-600 font-medium mb-1">
                  Nome
                </label>
                <InputText {...register('nome')} placeholder="Nome" className={"w-full md:w-30rem " + (errors.nome?.message ? 'p-invalid' : '')} style={{ padding: '1rem' }} />
                {errors.nome?.message ? <small className="p-error">{errors.nome.message}</small> : <small className="p-error"></small>}
              </div>

              <div className="flex flex-column gap-1 mb-1">
                <label className="block text-600 font-medium mb-1">
                  Sobrenome
                </label>
                <InputText {...register('sobrenome')} placeholder="Sobrenome" className={"w-full md:w-30rem " + (errors.sobrenome?.message ? 'p-invalid' : '')} style={{ padding: '1rem' }} />
                {errors.sobrenome?.message ? <small className="p-error">{errors.sobrenome.message}</small> : <small className="p-error"></small>}
              </div>

              <div className="flex flex-column gap-1 mb-1">
                <label className="block text-600 font-medium mb-1">
                  CPF
                </label>
                <InputMask {...register('cpf')} placeholder="Cpf" className={"w-full md:w-30rem " + (errors.cpf?.message ? 'p-invalid' : '')} style={{ padding: '1rem' }} mask='999.999.999-99' />
                {errors.cpf?.message ? <small className="p-error">{errors.cpf.message}</small> : <small className="p-error"></small>}
              </div>

              <div className="flex flex-column gap-1 mb-1">
                <label className="block text-600 font-medium mb-1">
                  E-mail
                </label>
                <InputText {...register('email')} placeholder="E-mail" className={"w-full md:w-30rem " + (errors.email?.message ? 'p-invalid' : '')} style={{ padding: '1rem' }} />
                {errors.email?.message ? <small className="p-error">{errors.email.message}</small> : <small className="p-error"></small>}
              </div>

              <div className="flex flex-column gap-1 mb-1">
                <label className="block text-600 font-medium mb-1">
                  Data de nascimento
                </label>
                <InputMask {...register('dataNascimento')} placeholder="Data de nascimento" className={"w-full md:w-30rem " + (errors.dataNascimento?.message ? 'p-invalid' : '')} style={{ padding: '1rem' }} mask='99/99/9999' />
                {errors.dataNascimento?.message ? <small className="p-error">{errors.dataNascimento.message}</small> : <small className="p-error"></small>}
              </div>

              <div className="flex flex-column gap-1 mb-1">
                <label className="block text-600 font-medium mb-1">
                  Senha
                </label>
                <Password {...register('senha')} onChange={(e) => setValue("senha", e.target.value)} placeholder="Senha" toggleMask feedback={false} className={"w-full " + (errors.senha?.message ? 'p-invalid' : '')} inputClassName="w-full p-3 md:w-30rem"></Password>
                {errors.senha?.message ? <small className="p-error">{errors.senha.message}</small> : <small className="p-error"></small>}
              </div>

              <div className="flex flex-column gap-1 mb-1">
                <label className="block text-600 font-medium mb-1">
                  Confirmação de senha
                </label>
                <Password {...register('confirmarSenha')} onChange={(e) => setValue("confirmarSenha", e.target.value)} placeholder="Confirmaçao de senha" toggleMask feedback={false} className={"w-full " + (errors.confirmarSenha?.message ? 'p-invalid' : '')} inputClassName="w-full p-3 md:w-30rem"></Password>
                {errors.confirmarSenha?.message ? <small className="p-error">{errors.confirmarSenha.message}</small> : <small className="p-error"></small>}
              </div>


              <div className="flex align-items-center justify-content-between mb-5 gap-5">
                <Link className="font-medium no-underline ml-2 text-right cursor-pointer" href="/login">Voltar</Link>
              </div>
              <Button label="Salvar" severity="secondary" className="w-full p-3 text-xl" loading={isLoading} onClick={handleSubmit(handleCadastrarGestor)}></Button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </>)
};

export default CadastroGestor;
