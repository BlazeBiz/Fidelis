import { createApp } from 'vue'
// import './style.css'
import App from './App.vue'
import router from './router'

/* import the fontawesome core */
import { library } from '@fortawesome/fontawesome-svg-core'
/* import font awesome icon component */
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'

/* import specific icons */
import { faPlus, faBan, faBinoculars, faPencil, faCircleUser } from '@fortawesome/free-solid-svg-icons'
import { faTrashCan, faFloppyDisk } from '@fortawesome/free-regular-svg-icons'

/* add icons to the library */
library.add(faPlus, faTrashCan, faFloppyDisk, faBan, faBinoculars, faPencil, faCircleUser);

createApp(App)
    .use(router)
    .component('font-awesome-icon', FontAwesomeIcon)
    .mount('#app');
