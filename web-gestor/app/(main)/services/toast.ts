import { RefObject } from "@fullcalendar/core/preact";
import { Toast } from "primereact/toast";

export const toastSuccess = (toast: RefObject<Toast>, title: string) => {
  toast.current?.show({
    severity: 'success',
    summary: 'Sucesso',
    detail: title,
    life: 3000
  });
}

export const toastFail = (toast: RefObject<Toast>, title: string) => {
  toast.current?.show({
    severity: 'error',
    summary: 'Falha',
    detail: title,
    life: 3000
  });
}
