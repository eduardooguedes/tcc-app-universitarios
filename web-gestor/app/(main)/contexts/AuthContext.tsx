"use client";

import { createContext, useEffect, useState } from "react";
import { setCookie, parseCookies, destroyCookie } from 'nookies'
import { ScriptProps } from "next/script";

type User = {
  cpf: string;
  dataDeCadastro: string;
  dataDeNascimento: string;
  email: string;
  emailConfirmado: boolean;
  id: string;
  idNivelGestao: number;
  idSituacao: number;
  nome: string;
  sobrenome: string;
}

type SignInData = {
  email: string;
  senha: string;
}

type AuthContextType = {
  isAuthenticated: boolean;
  user: User | null;
  signIn: (data: SignInData) => Promise<User>
}

export const AuthContext = createContext({} as AuthContextType)

export function AuthProvider({ children }: ScriptProps) {
  const [user, setUser] = useState<User | null>(null)

  const isAuthenticated = !!user;

  useEffect(() => {
    const { 'Universitario.token': token } = parseCookies()

    if (token) {

    }
  }, [])

  async function signIn({ email, senha }: SignInData) {

    const res = await fetch(`${process.env.NEXT_PUBLIC_URL_API}/v1/auth/login/gestor`, {
      method: "POST",
      body: JSON.stringify({ email, senha }),
      headers: { "Content-type": "application/json; charset=UTF-8" }
    })

    if (res.ok) {

      const expireTime = 60 * 60 * 8;
      const json = await res.json();
      const { gestor: user, estabelecimentos: estabelecimentos, token } = json.data;

      destroyCookie(undefined, 'Universitario.token')
      destroyCookie(undefined, 'Universitario.user')
      destroyCookie(undefined, 'Universitario.estabelecimento')

      setCookie(undefined, 'Universitario.token', token, {
        maxAge: expireTime,
      })

      setCookie(undefined, 'Universitario.user', JSON.stringify(user), {
        maxAge: expireTime,
      })
      if (estabelecimentos && estabelecimentos.length) {
        setCookie(undefined, 'Universitario.estabelecimento', estabelecimentos[0].id, {
          maxAge: expireTime,
        })
      }

      setUser(user);

      return user;
    }
    throw new Error('Usuário ou senha inválida');
  }

  return (
    <AuthContext.Provider value={{ user, isAuthenticated, signIn }}>
      {children}
    </AuthContext.Provider>
  )
}
