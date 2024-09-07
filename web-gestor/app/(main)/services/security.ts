import { parseCookies } from 'nookies';

export const validarUsuario = (ctx: any, validaEstabelecimento: boolean = true) => {

  const { ['Universitario.token']: token } = parseCookies(ctx)

  if (!token) {
    return '/login';
  }

  const { ['Universitario.user']: user } = parseCookies(ctx)
  const gestor = JSON.parse(user)

  if (gestor && !gestor.emailConfirmado) {
    return '/confirmar_cadastro';
  }

  if (validaEstabelecimento) {
    const { ['Universitario.estabelecimento']: estabelecimento } = parseCookies(ctx)

    if (!estabelecimento) {
      return '/cadastro_estabelecimento';
    }
  }

  return null;
};
