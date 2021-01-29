import { MDCSnackbar } from '@material/snackbar';

export function init(elem) {
    elem._snackbar = new MDCSnackbar(elem);
}

export function destroy(elem) {
    elem._snackbar.destroy();
}

export function open(elem) {
    elem._snackbar.open();
}
