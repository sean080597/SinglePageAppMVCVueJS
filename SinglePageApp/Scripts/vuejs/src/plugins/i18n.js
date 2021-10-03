import Vue from 'vue'
import VueI18n from 'vue-i18n'
// Import all langs
import en from '../langs/en.json'
import ja from '../langs/ja.json'

Vue.use(VueI18n)

export const i18n = new VueI18n({
    locale: 'en',
    fallbackLocale: 'en',
    messages: {
        en
    }
})