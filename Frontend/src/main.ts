import { createApp } from 'vue';
import App from './App.vue';
import './registerServiceWorker';
import router from './router';

// Bootstrap
import jQuery from 'jquery';
import 'bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';
(window as any).$ = jQuery;
(window as any).jQuery = jQuery;

createApp(App).use(router).mount('#app');
