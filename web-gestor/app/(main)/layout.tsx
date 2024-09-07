'use client';
import { Metadata } from 'next';
import Layout from '../../layout/layout';
import { useRouter } from 'next/navigation';
import { validarUsuario } from './services/security';
import { useEffect, useState } from 'react';

interface AppLayoutProps {
  children: React.ReactNode;
}

export const metadata: Metadata = {
  title: 'Universitario',
  description: '',
  robots: { index: false, follow: false },
  viewport: { initialScale: 1, width: 'device-width' },
  icons: {
    icon: '/favicon.ico'
  }
};

export default function AppLayout({ children }: AppLayoutProps) {

  const [isLoading, setIsLoading] = useState(true);
  const router = useRouter();

  useEffect(() => {

    const res = validarUsuario(null);

    if (res) {
      router.push(res);
    } else {
      setIsLoading(false)
    }
  }, []);


  return !isLoading ? <Layout>{children}</Layout> : <></>;
}