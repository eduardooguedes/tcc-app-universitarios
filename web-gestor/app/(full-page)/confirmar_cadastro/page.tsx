'use client';
import { useRouter } from 'next/navigation';
import { parseCookies, setCookie } from 'nookies';
import { Button } from 'primereact/button';
import { Toast } from 'primereact/toast';
import { classNames } from 'primereact/utils';
import { useEffect, useRef, useState } from 'react';
import PinInput from 'react-pin-input';
import { z } from "zod";
import { api } from '../../(main)/services/api';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { toastFail } from '../../(main)/services/toast';

const ConfirmarCadastro = () => {

  const [isLoading, setIsLoading] = useState(false);
  const router = useRouter();
  const [email, setEmail] = useState('');
  const [tempo, setTempo] = useState(1);
  const toast = useRef<Toast>(null);

  useEffect(() => {
    const fetchData = async () => {
      enviarEmail();
    }
    fetchData()
      .catch(console.error);
  }, []);

  useEffect(() => {
    let interval: any = null;

    interval = setInterval(() => {
      if (tempo > 0) setTempo(seconds => seconds - 1);

    }, 1000);

    return () => clearInterval(interval);
  }, [tempo]);

  async function enviarEmail() {
    setIsLoading(true)

    const { ['Universitario.user']: user } = parseCookies()

    if (!user) {
      router.push('/login');
      return;
    }

    const gestor = JSON.parse(user);

    const body = {
      novoEmail: gestor.email
    }

    try {
      setEmail(gestor.email)

      const res = await api.post(`/v1/usuario/email`, body);

      const data = res.data.data;

      setTempo(data)
      setIsLoading(false)
    } catch (err) {
      toastFail(toast, err?.response?.data?.erro ?? 'Falha ao comunicar com o servidor')
    }
  }

  function secondsToTime(timer: number) {
    const divisao_minutos = timer % (60 * 60);
    const minutos = Math.floor(divisao_minutos / 60);

    const divisao_segundos = divisao_minutos % 60;
    const segundos = Math.ceil(divisao_segundos);

    const obj = {
      "m": minutos.toString().padStart(2, '0'),
      "s": segundos.toString().padStart(2, '0')
    };
    return obj;
  }

  async function handleConfirmarEmail(data: CreateConfirmacaoFormSchema) {
    setIsLoading(true)

    const body = {
      codigo: data.codigo
    }

    await api.post(`/v1/usuario/confirmarCodigoRecebidoEmail`, body)
      .then(data => {
        const { ['Universitario.user']: user } = parseCookies();

        const gestor = JSON.parse(user);

        gestor.emailConfirmado = true;

        setCookie(undefined, 'Universitario.user', JSON.stringify(gestor))

        router.push('/cadastro_estabelecimento');
      })
      .catch(err => {
        toastFail(toast, err?.response?.data?.erro ?? 'Falha ao comunicar com o servidor')
      });

    setIsLoading(false)
  }

  const createConfirmacaoFormSchema = z.object({
    codigo: z.string()
      .min(1, "Obrigatório informar codigo."),
  })

  type CreateConfirmacaoFormSchema = z.infer<typeof createConfirmacaoFormSchema>

  const createConfirmacaoForm = useForm<CreateConfirmacaoFormSchema>({
    resolver: zodResolver(createConfirmacaoFormSchema)
  });

  const {
    handleSubmit,
    setValue
  } = createConfirmacaoForm

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

            <span>Enviamos um código de verificação para <br />
              <strong>{email}</strong><br />
              Insira-o abaixo. <br /><br />
              {!isLoading && tempo > 0 && <span>Expira em {secondsToTime(tempo).m}:{secondsToTime(tempo).s}</span>}
              {!isLoading && tempo == 0 && <Button severity="secondary" className="w-full p-3 text-xl" onClick={() => enviarEmail()}>Reenviar código</Button>}
            </span>

            <PinInput
              length={6}
              initialValue=""
              onChange={(value, index) => { setValue("codigo", value); }}
              type="numeric"
              inputMode="number"
              style={{ padding: '10px' }}
              inputFocusStyle={{ borderColor: '#ff3300' }}
              onComplete={(value, index) => { setValue("codigo", value); }}
              autoSelect={true}
              regexCriteria={/^[ A-Za-z0-9_@./#&+-]*$/}
            />

            <div>

              <Button label="Concluir" severity="secondary" className="w-full p-3 text-xl" loading={isLoading} onClick={handleSubmit(handleConfirmarEmail)}></Button>

            </div>
          </div>
        </div>
      </div>
    </div>
  </>)
};

export default ConfirmarCadastro;