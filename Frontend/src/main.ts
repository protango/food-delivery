import { createApp } from 'vue';
import App from './App.vue';
import './registerServiceWorker';
import router from './router';

// Bootstrap
import 'bootstrap';

createApp(App).use(router).mount('#app');
