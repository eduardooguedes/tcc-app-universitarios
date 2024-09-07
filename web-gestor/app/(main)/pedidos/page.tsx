'use client';
import { parseCookies } from 'nookies';
import { Button } from 'primereact/button';
import { Calendar } from 'primereact/calendar';
import { DataView } from 'primereact/dataview';
import { Dialog } from 'primereact/dialog';
import { Tag } from 'primereact/tag';
import { Toast } from 'primereact/toast';
import { Toolbar } from 'primereact/toolbar';
import { useEffect, useRef, useState } from 'react';
import { IPedido } from '../models/IPedido';
import { api } from '../services/api';
import FullCalendar from '@fullcalendar/react'
import dayGridPlugin from '@fullcalendar/daygrid'
import ptLocale from '@fullcalendar/core/locales/pt-br';
import interactionPlugin from '@fullcalendar/interaction';
import { toastFail, toastSuccess } from '../services/toast';

export const Pedidos = () => {

  const [isLoading, setIsLoading] = useState(false);
  const [novosPedidos, setNovosPedidos] = useState<any[]>([]);
  const [pedidos, setPedidos] = useState<IPedido[]>([]);
  const [pedidosFiltrados, setPedidosFiltrados] = useState<IPedido[]>([]);
  const [situacoes, setSituacoes] = useState<string[]>();
  const [situacao, setSituacao] = useState<string>();
  const [aceitarPedidoDialog, setAceitarPedidoDialog] = useState(false);
  const [entregarPedidoDialog, setEntregarPedidoDialog] = useState(false);
  const [rejeitarPedidoDialog, setRejeitarPedidoDialog] = useState(false);
  const [cancelarPedidoDialog, setCancelarPedidoDialog] = useState(false);

  const [data, setData] = useState(new Date());
  const [pedido, setPedido] = useState<IPedido | null>();
  const [idEstabelecimento, setIdEstabelecimento] = useState('');

  const toast = useRef<Toast>(null);

  useEffect(() => {

    const { ['Universitario.estabelecimento']: estabelecimento } = parseCookies();

    setIdEstabelecimento(estabelecimento);
  }, []);

  useEffect(() => {

    setIsLoading(true);

    if (idEstabelecimento)
      api.get(`/v1/estabelecimento/${idEstabelecimento}/pedido/resumoMensal`).then((res) => {
        const _pedidos = res.data.data.dias;

        // corrigir para buscar os pedidos do mês
        setNovosPedidos(_pedidos.map((x: any) => (
          {
            title: '1 - pedido novo',
            date: `2024-03-${x.diaDoMes}`,
            backgroundColor: 'black',
            borderColor: 'black'
          })));
      });

    setIsLoading(false);
  }, [idEstabelecimento, data]);

  useEffect(() => {

    setIsLoading(true);

    if (idEstabelecimento)
      api.get(`/v1/estabelecimento/${idEstabelecimento}/pedido/${data.toISOString().split("T")[0]}`).then((res) => {
        const _pedidos = res.data.data;

        setPedidos(_pedidos);
        setPedidosFiltrados(_pedidos);

        const unique = _pedidos.map((x: any) => x.situacao.descricao.toUpperCase()).filter((x: any, index: number, array: any) => array.indexOf(x) === index);

        setSituacoes(unique);
        setSituacao('');
      });

    setIsLoading(false);
  }, [idEstabelecimento, data]);

  const findIndexById = (id: string) => {
    let index = -1;
    for (let i = 0; i < (pedidos as any)?.length; i++) {
      if ((pedidos as any)[i].id === id) {
        index = i;
        break;
      }
    }

    return index;
  };

  const filtrarSituacao = (situacaoFiltro: string) => {

    let clear = false;

    if (situacao == situacaoFiltro) {
      clear = true;
      setSituacao('')
    }
    else {
      setSituacao(situacaoFiltro)
    }

    setPedidosFiltrados(pedidos.filter((x: any) => clear || x.situacao.descricao.toUpperCase() == situacaoFiltro));
  }

  const aceitarPedido = (pedido: IPedido) => {
    setPedido(pedido);
    setAceitarPedidoDialog(true)
  }

  const aceitarRejeitarPedido = () => {
    api.post(`/v1/estabelecimento/${idEstabelecimento}/pedido/${pedido?.id}/aceitar`)
      .then(res => {
        const _pedidos = [...(pedidos as any)];

        const _pedido = res.data.data;
        const index = findIndexById(_pedido.id);

        _pedidos[index] = _pedido;

        setPedidos(_pedidos);

        toastSuccess(toast, 'Pedido aceito com sucesso');
      })
      .catch(err => {
        toastFail(toast, err?.response?.data?.erro ?? 'Falha ao comunicar com o servidor');
      })
      .finally(() => {
        setAceitarPedidoDialog(false);
        setPedido(null);
      });
  };

  const hideAceitarPedidoDialog = () => {
    setPedido(null);
    setAceitarPedidoDialog(false);
  };

  const aceitarDialogFooter = (
    <>
      <Button label="Cancelar" severity="secondary" icon="pi pi-times" text onClick={hideAceitarPedidoDialog} />
      <Button label="Sim" severity="success" icon="pi pi-check" loading={isLoading} onClick={aceitarRejeitarPedido} />
    </>
  );

  const entregarPedido = (pedido: IPedido) => {
    setPedido(pedido);
    setEntregarPedidoDialog(true)
  }

  const hideEntregarPedidoDialog = () => {
    setPedido(null);
    setEntregarPedidoDialog(false);
  };

  const confirmarEntregarPedido = () => {
    api.post(`/v1/estabelecimento/${idEstabelecimento}/pedido/${pedido?.id}/entregar`)
      .then(res => {
        const _pedidos = [...(pedidos as any)];

        const _pedido = res.data.data;
        const index = findIndexById(_pedido.id);

        _pedidos[index] = _pedido;

        setPedidos(_pedidos);

        toastSuccess(toast, 'Pedido entregue com sucesso');
      })
      .catch(err => {
        toastFail(toast, err?.response?.data?.erro ?? 'Falha ao comunicar com o servidor');
      })
      .finally(() => {
        setEntregarPedidoDialog(false);
        setPedido(null);
      });
  };

  const entregarDialogFooter = (
    <>
      <Button label="Cancelar" severity="secondary" icon="pi pi-times" text onClick={hideEntregarPedidoDialog} />
      <Button label="Sim" severity="info" icon="pi pi-check" loading={isLoading} onClick={confirmarEntregarPedido} />
    </>
  );

  const rejeitarPedido = (pedido: IPedido) => {
    setPedido(pedido);
    setRejeitarPedidoDialog(true)
  }

  const hideRejeitarPedidoDialog = () => {
    setPedido(null);
    setRejeitarPedidoDialog(false);
  };

  const confirmarRejeitarPedido = () => {
    api.post(`/v1/estabelecimento/${idEstabelecimento}/pedido/${pedido?.id}/rejeitar`, { motivo: 'teste teste teste teste ' })
      .then(res => {
        const _pedidos = [...(pedidos as any)];

        const _pedido = res.data.data;
        const index = findIndexById(_pedido.id);

        _pedidos[index] = _pedido;

        setPedidos(_pedidos);

        toastSuccess(toast, 'Pedido rejeitado com sucesso');
      })
      .catch(err => {
        toastFail(toast, err?.response?.data?.erro ?? 'Falha ao comunicar com o servidor');
      })
      .finally(() => {
        setRejeitarPedidoDialog(false);
        setPedido(null);
      });
  };

  const rejeitarDialogFooter = (
    <>
      <Button label="Cancelar" severity="secondary" icon="pi pi-times" text onClick={hideRejeitarPedidoDialog} />
      <Button label="Ok, rejeitar" severity="danger" icon="pi pi-check" loading={isLoading} onClick={confirmarRejeitarPedido} />
    </>
  );

  const cancelarPedido = (pedido: IPedido) => {
    setPedido(pedido);
    setCancelarPedidoDialog(true)
  }

  const hideCancelarPedidoDialog = () => {
    setPedido(null);
    setCancelarPedidoDialog(false);
  };

  const confirmarCancelarPedido = () => {
    api.post(`/v1/estabelecimento/${idEstabelecimento}/pedido/${pedido?.id}/desfazerEntrega`)
      .then(res => {
        const _pedidos = [...(pedidos as any)];

        const _pedido = res.data.data;
        const index = findIndexById(_pedido.id);

        _pedidos[index] = _pedido;

        setPedidos(_pedidos);

        toastSuccess(toast, 'Pedido cancelado com sucesso');
      })
      .catch(err => {
        toastFail(toast, err?.response?.data?.erro ?? 'Falha ao comunicar com o servidor');
      })
      .finally(() => {
        setCancelarPedidoDialog(false);
        setPedido(null);
      });
  };

  const cancelarDialogFooter = (
    <>
      <Button label="Cancelar" severity="secondary" icon="pi pi-times" text onClick={hideCancelarPedidoDialog} />
      <Button label="Sim" severity="danger" icon="pi pi-check" loading={isLoading} onClick={confirmarCancelarPedido} />
    </>
  );

  const centerToolbarTemplate = () => {
    return (
      <div className="flex flex-column gap-2 my-2">
        <label>Selecione uma data:</label>
        <Calendar value={data} onChange={(e) => setData(e.value)} dateFormat="dd/mm/yy" />
      </div>
    );
  };

  const header = (
    <div className="flex flex-column md:flex-row md:justify-content-between md:align-items-center">
      <h5 className="m-0">Pedidos</h5>
    </div>
  );

  const formatCurrency = (value: number) => {
    return value.toLocaleString('pt-BR', {
      style: 'currency',
      currency: 'BRL'
    });
  };

  const gridItem = (pedido: IPedido) => {
    return (
      <div className="col-12 sm:col-6 lg:col-12 xl:col-4 p-2" key={pedido.id}>
        <div className="p-4 border-1 surface-border surface-card border-round">
          <div className="flex flex-wrap align-items-center justify-content-between gap-2">
            <div className="flex align-items-center gap-2">
              <i className="pi pi-clock"></i>
              <span className="font-semibold">{pedido.horarioDeRetirada.substring(0, 5)}</span>
            </div>
            <Tag value={pedido.situacao.descricao} style={{ background: `#${pedido.situacao.codigoDaCorEmHexadecimal}` }}></Tag>
          </div>
          <div className="flex flex-column align-items-center gap-3 py-5">
            <div className="text-2xl font-bold">{pedido.cliente.nome}</div>
          </div>
          <div>
            {pedido.produtos.map((x: any) =>
              <div key={x.id} className='mb-3'>
                <div className="flex align-items-center justify-content-between">
                  <span>
                    {x.quantidade}x {x.nome}
                  </span>
                  <span>
                    R$ {x.precoTotal.toFixed(2).replace(',', '').replace('.', ',')}
                  </span>
                </div>
                {x.adicionais.map((y: any) =>
                  <div key={y.id}>
                    <div className="flex align-items-center justify-content-between">
                      <span>
                        {y.quantidade}x {y.nome}
                      </span>
                      <span>
                        R$ {y.precoTotal.toFixed(2).replace(',', '').replace('.', ',')}
                      </span>
                    </div>
                  </div>)}
              </div>)}
          </div>
          <hr />
          <div className="flex align-items-center justify-content-between">
            <span className="text-2xl font-semibold">Total:</span>
            <span className="text-2xl font-semibold">{formatCurrency(pedido.valorTotal)}</span>
          </div>
          <hr />
          <div className="flex align-items-center justify-content-end gap-4">
            {pedido.permitidoRejeitar && <Button severity='danger' onClick={() => rejeitarPedido(pedido)}>Rejeitar</Button>}
            {pedido.permitidoCancelarEntrega && <Button severity='danger' onClick={() => cancelarPedido(pedido)}>Cancelar</Button>}
            {pedido.permitidoEntregar && <Button severity='info' onClick={() => entregarPedido(pedido)}>Entregar</Button>}
            {pedido.permitidoAceitar && <Button severity='success' onClick={() => aceitarPedido(pedido)}>Aceitar</Button>}
          </div>
        </div>
      </div>
    );
  };

  return (
    <div className="card">
      <Toast ref={toast} />
      <FullCalendar
        plugins={[dayGridPlugin, interactionPlugin]}
        initialView="dayGridMonth"
        locale={ptLocale}
        editable={true}
        eventClick={() => alert(123)}
        selectable={true}
        selectMirror={true}
        dateClick={function (info) {
          setData(info.date)
        }}
        events={novosPedidos}
      />
      <Toolbar className="mb-4" center={centerToolbarTemplate}></Toolbar>
      {situacoes?.length > 0 &&
        <div className="flex align-items-center justify-content-center gap-2 mb-4">
          {situacoes.map((x: any) => <Button style={{ background: situacao == x ? "#ff3300" : "#bbb", borderColor: situacao == x ? "#ff3300" : "#bbb" }} key={x} onClick={() => filtrarSituacao(x)}>{x}</Button>)}
        </div>
      }
      <DataView value={pedidosFiltrados} itemTemplate={gridItem} header={header} emptyMessage="Nenhum pedido encontrado." loading={isLoading} />

      <Dialog visible={rejeitarPedidoDialog} style={{ width: '450px' }} header="Rejeitar?" modal footer={rejeitarDialogFooter} onHide={hideRejeitarPedidoDialog}>
        <div className="flex align-items-center justify-content-center">
          <span>
            informe ao cliente o motivo:
          </span>
        </div>
      </Dialog>

      <Dialog visible={entregarPedidoDialog} style={{ width: '450px' }} header="Entregar pedido?" modal footer={entregarDialogFooter} onHide={hideEntregarPedidoDialog}>
        <div className="flex align-items-center justify-content-center">
          <span>
            Esta ação nao poderá ser desfeita.
          </span>
        </div>
      </Dialog>

      <Dialog visible={cancelarPedidoDialog} style={{ width: '450px' }} header="Cancelar Entrega?" modal footer={cancelarDialogFooter} onHide={hideCancelarPedidoDialog}>
        <div className="flex align-items-center justify-content-center">
          <span>
            Confirme para cancelar a entrega desse pedido.
          </span>
        </div>
      </Dialog>

      <Dialog visible={aceitarPedidoDialog} style={{ width: '450px' }} header="Aceitar?" modal footer={aceitarDialogFooter} onHide={hideAceitarPedidoDialog}>
        <div className="flex align-items-center justify-content-center">
          <span>
            Confirme para aceitar esse pedido.
          </span>
        </div>
      </Dialog>

    </div>
  )
};

export default Pedidos;