'use client';
import { Button } from 'primereact/button';
import { Toast } from 'primereact/toast';
import { classNames } from 'primereact/utils';
import { useEffect, useRef, useState } from 'react';
import { api } from '../../(main)/services/api';
import { parseCookies } from 'nookies';
import router from 'next/router';
import { toastFail, toastSuccess } from '../../(main)/services/toast';
import Link from 'next/link';

const CadastroEstabelecimentoLogo = () => {

  const [isLoading, setIsLoading] = useState(false);
  const [nome, setNome] = useState('');
  const [idEstabelecimento, setIdEstabelecimento] = useState('');
  const [logoEstabelecimento, setLogoEstabelecimento] = useState('');
  const toast = useRef<Toast>(null);

  useEffect(() => {

    const { ['Universitario.estabelecimento']: estabelecimento } = parseCookies();

    if (!estabelecimento) {
      router.push('/');
      return;
    }

    setIdEstabelecimento(estabelecimento);

    const { ['Universitario.user']: user } = parseCookies();

    const gestor = JSON.parse(user);

    setNome(gestor.nome);

  }, []);

  async function handleCadastrarEstabelecimentoLogo() {

    setIsLoading(true)

    if (!logoEstabelecimento) return;

    let blob = await fetch(logoEstabelecimento).then(r => r.blob());

    const body = new FormData();
    body.append("logo", blob, 'logo')

    api.post(`/v1/estabelecimento/${idEstabelecimento}/logo`, body)
      .then(res => {
        router.push('/');

        toastSuccess(toast, 'Logo cadastrado com sucesso');
      })
      .catch(err => {
        toastFail(toast, err?.response?.data?.erro ?? 'Falha ao comunicar com o servidor')
      })
      .finally(() => setIsLoading(false))
  }

  function getFile(logo: File) {
    setLogoEstabelecimento(URL.createObjectURL(logo))
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

            <h4>Bem vindo,<span style={{ marginLeft: '5px', color: 'var(--primary-color)' }}>{nome}</span></h4>
            <div>

              <Button label="Concluir" severity="secondary" className="w-full p-3 text-xl" loading={isLoading} onClick={handleCadastrarEstabelecimentoLogo}></Button>

              <div className="flex align-items-center justify-content-between mb-5 gap-5">
                <Link className="font-medium no-underline ml-2 text-right cursor-pointer" href="/">Prosseguir sem logo</Link>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </>)
};

export default CadastroEstabelecimentoLogo;