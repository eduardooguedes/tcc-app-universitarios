/* eslint-disable @next/next/no-img-element */

import { AppMenuItem } from '@/types';
import { useContext } from 'react';
import AppMenuitem from './AppMenuitem';
import { LayoutContext } from './context/layoutcontext';
import { MenuProvider } from './context/menucontext';
import { faBurger, faUser, faBuilding, faListCheck, faClock } from '@fortawesome/free-solid-svg-icons';

const AppMenu = () => {
  const { layoutConfig } = useContext(LayoutContext);

  const model: AppMenuItem[] = [
    // {
    //   label: 'Home',
    //   items: [{ label: 'Dashboard', icon: faChartLine, to: '/' }]
    // },
    {
      index: 0,
      items: [
        { label: 'Pedidos', icon: faListCheck, to: '/pedidos' },
        { label: 'Produtos', icon: faBurger, to: '/produtos' },
        { label: 'Horário de funcionamento', icon: faClock, to: '/horarios' },
        { label: 'Gestor', icon: faUser, to: '/gestor' },
        { label: 'Estabelecimento', icon: faBuilding, to: '/estabelecimento' },
      ]
    },
  ];

  return (
    <MenuProvider>
      <ul className="layout-menu">
        {model.map((item, i) => {
          return !item?.seperator ? <AppMenuitem item={item} root={true} index={i} key={item.index} /> : <li className="menu-separator"></li>;
        })}
      </ul>
    </MenuProvider>
  );
};

export default AppMenu;
