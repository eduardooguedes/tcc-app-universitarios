/* eslint-disable @next/next/no-img-element */

import { AppTopbarRef } from '@/types';
import Link from 'next/link';
import { destroyCookie, parseCookies } from 'nookies';
import { Button } from 'primereact/button';
import { Dialog } from 'primereact/dialog';
import { Menu } from 'primereact/menu';
import { classNames } from 'primereact/utils';
import React, { forwardRef, useContext, useEffect, useImperativeHandle, useRef, useState } from 'react';
import { api } from '../app/(main)/services/api';
import { LayoutContext } from './context/layoutcontext';
import { useRouter } from 'next/navigation';

const AppTopbar = forwardRef<AppTopbarRef>((props, ref) => {
  const { layoutConfig, layoutState, onMenuToggle, showProfileSidebar } = useContext(LayoutContext);
  const [logoutDialog, setLogoutDialog] = useState(false);

  const menubuttonRef = useRef(null);
  const topbarmenuRef = useRef(null);
  const topbarmenubuttonRef = useRef(null);
  const menu = useRef<Menu>(null);
  const router = useRouter();

  useImperativeHandle(ref, () => ({
    menubutton: menubuttonRef.current,
    topbarmenu: topbarmenuRef.current,
    topbarmenubutton: topbarmenubuttonRef.current
  }));

  const toggleMenu = (event: React.MouseEvent<HTMLButtonElement>) => {
    menu.current?.toggle(event);
  };

  const [logo, setLogo] = useState('');
  const [nomeFantasia, setNomeFantasia] = useState('');

  const hideLogoutDialog = () => {
    setLogoutDialog(false);
  };

  const logoutDialogFooter = (
    <>
      <Button label="Não" severity="secondary" icon="pi pi-times" text onClick={hideLogoutDialog} />
      <Button label="Sim" severity="secondary" icon="pi pi-check" onClick={() => lougout()} />
    </>
  );

  const lougout = () => {
    destroyCookie(undefined, 'Universitario.token')
    destroyCookie(undefined, 'Universitario.user')
    destroyCookie(undefined, 'Universitario.estabelecimento')

    router.push('/login')
  }

  useEffect(() => {
    const fetchData = async () => {

      const { ['Universitario.estabelecimento']: estabelecimento } = parseCookies();

      api.get(`/v1/estabelecimento/${estabelecimento}`).then(res => { setNomeFantasia(res.data.data.nomeFantasia); setLogo(res.data.data.logo) });
    }

    fetchData()
      .catch(console.error);
  }, []);

  return (
    <div className="layout-topbar">
      <Link href="/" className="layout-topbar-logo">
        {logo && <img src={logo} height={'35px'} alt="logo" />}
        <span>{nomeFantasia}</span>
      </Link>

      <button ref={menubuttonRef} type="button" className="p-link layout-menu-button layout-topbar-button" onClick={onMenuToggle}>
        <i className="pi pi-bars" />
      </button>

      <button ref={topbarmenubuttonRef} type="button" className="p-link layout-topbar-menu-button layout-topbar-button" onClick={showProfileSidebar}>
        <i className="pi pi-ellipsis-v" />
      </button>

      <div ref={topbarmenuRef} className={classNames('layout-topbar-menu', { 'layout-topbar-menu-mobile-active': layoutState.profileSidebarVisible })}>

        <button type="button" className="p-link layout-topbar-button" onClick={() => setLogoutDialog(true)}>
          <i className="pi pi-power-off"></i>
        </button>

        <Dialog visible={logoutDialog} style={{ width: '450px' }} header="Sair" modal footer={logoutDialogFooter} onHide={hideLogoutDialog}>
          <div className="flex align-items-center justify-content-center">
            <i className="pi pi-exclamation-triangle mr-3" style={{ fontSize: '2rem' }} />
            <span>
              Deseja realmente deslogar do sistema?
            </span>
          </div>
        </Dialog>
      </div>
    </div>
  );
});

AppTopbar.displayName = 'AppTopbar';

export default AppTopbar;
