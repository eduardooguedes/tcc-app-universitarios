'use client';
import { zodResolver } from '@hookform/resolvers/zod';
import { useRouter } from 'next/navigation';
import { Button } from 'primereact/button';
import { InputText } from 'primereact/inputtext';
import { Password } from 'primereact/password';
import { classNames } from 'primereact/utils';
import { useContext, useRef, useState } from 'react';
import { useForm } from 'react-hook-form';
import { z } from 'zod';
import { AuthContext } from '../../(main)/contexts/AuthContext';
import { Toast } from 'primereact/toast';
import Link from 'next/link';
import { toastFail } from '../../(main)/services/toast';

const LoginPage = () => {

  const { signIn } = useContext(AuthContext);
  const router = useRouter();
  const [isLoading, setIsLoading] = useState(false);
  const toast = useRef<Toast>(null);

  const loginFormSchema = z.object({
    login: z.string()
      .min(1, "Obrigatório informar login."),
    senha: z.string()
      .min(1, "Obrigatório informar senha.")
  });

  type LoginFormData = z.infer<typeof loginFormSchema>

  const createUserForm = useForm<LoginFormData>({
    resolver: zodResolver(loginFormSchema)
  });

  const {
    handleSubmit,
    register,
    formState: { errors },
    setValue
  } = createUserForm

  async function handleLogin(data: LoginFormData) {
    setIsLoading(true)
    signIn({ email: data.login, senha: data.senha })
      .then(res => router.push('/'))
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
            background: 'linear-gradient(180deg, var(--primary-color) 10%, rgba(33, 150, 243, 0) 30%)'
          }}
        >
          <div className="w-full surface-card py-8 px-5 sm:px-8" style={{ borderRadius: '53px' }}>
            <div className="text-center mb-5">
              <img src="/layout/images/Universitario_logo_laranja.png" alt="Image" height="100" className="mb-3" />
            </div>

            <div>
              <div className="flex flex-column gap-2 mb-5">
                <label htmlFor="email1" className="block text-900 text-xl font-medium mb-2">
                  Login
                </label>
                <InputText {...register('login')} placeholder="Login" className={"w-full md:w-30rem " + (errors.login?.message ? 'p-invalid' : '')} style={{ padding: '1rem' }} />
                {errors.login?.message ? <small className="p-error">{errors.login.message}</small> : <small className="p-error"></small>}
              </div>

              <div className="flex flex-column gap-2 mb-5">
                <label htmlFor="password1" className="block text-900 font-medium text-xl mb-2">
                  Senha
                </label>
                <Password {...register('senha')} onChange={(e) => setValue("senha", e.target.value)} placeholder="Senha" toggleMask feedback={false} className={"w-full " + (errors.senha?.message ? 'p-invalid' : '')} inputClassName="w-full p-3 md:w-30rem"></Password>
                {errors.senha?.message ? <small className="p-error">{errors.senha.message}</small> : <small className="p-error"></small>}
              </div>


              <div className="flex align-items-center justify-content-between mb-5 gap-5">
                <a className="font-medium no-underline ml-2 text-right cursor-pointer" style={{ color: 'var(--primary-color)' }}>
                  Esqueci minha senha
                </a>
                <span>Novo usuário?
                  <Link className="font-medium no-underline ml-2 text-right cursor-pointer" href="/cadastro_gestor">Cadastre-se</Link>
                </span>
              </div>
              <Button label="Entrar" severity="secondary" className="w-full p-3 text-xl" loading={isLoading} onClick={handleSubmit(handleLogin)}></Button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </>
  );
};

export default LoginPage;
