import { Metadata } from 'next';
import React from 'react';
import { AuthProvider } from '../(main)/contexts/AuthContext';

interface SimpleLayoutProps {
  children: React.ReactNode;
}

export const metadata: Metadata = {
  title: 'Universitario',
  description: ''
};

export default function SimpleLayout({ children }: SimpleLayoutProps) {
  return (
    <React.Fragment>
      <AuthProvider>
        {children}
      </AuthProvider>
    </React.Fragment>
  );
}
