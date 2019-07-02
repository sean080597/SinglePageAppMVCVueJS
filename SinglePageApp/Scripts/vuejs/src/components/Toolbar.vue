<template>
  <v-toolbar>
    <!-- <v-toolbar-side-icon></v-toolbar-side-icon> -->
    <v-toolbar-title>
      <router-link class="btn_no_effect" to="/">AppName</router-link>
    </v-toolbar-title>
    <v-spacer></v-spacer>
    <v-toolbar-items class="hidden-sm-and-down">
      <!-- <v-btn flat to="/department">Departments</v-btn>
      <v-btn flat to="/employee">Employees</v-btn>
      <v-btn flat to="/readwritebinfile">Bin File</v-btn>
      <v-btn flat to="/try_department">Departments Lab</v-btn>
      <v-btn flat to="/try_employee">Employees Lab</v-btn>
      <v-btn flat to="/chart">Chart</v-btn> -->
      <v-btn flat to="/svsetting">SV Setting</v-btn>
      <v-btn flat to="/svserver">SV Server</v-btn>
      <v-menu open-on-hover transition="slide-y-transition">
        <template v-slot:activator="{ on }">
          <v-btn flat color="indigo" small v-on="on">Languages</v-btn>
        </template>

        <v-list>
          <v-list-tile
            v-for="(item, index) in languages"
            :key="index"
            avatar
            @click="setLocale(item.locale)"
          >
            <v-list-tile-avatar>
              <span :class="'flag-icon flag-icon-' + item.flag + ' flag-icon-squared'"></span>
            </v-list-tile-avatar>
            <v-list-tile-content>
              <v-list-tile-title v-html="item.name"></v-list-tile-title>
            </v-list-tile-content>
          </v-list-tile>
        </v-list>
      </v-menu>
    </v-toolbar-items>
  </v-toolbar>
</template>

<script>
export default {
  data() {
    return {
      languages: [
        { name: "English", flag: "us", locale: "en" },
        { name: "Japanese", flag: "jp", locale: "ja" }
      ]
    };
  },

  methods: {
    setLocale(locale) {
      import(`../langs/${locale}.json`).then(msgs => {
        this.$i18n.setLocaleMessage(locale, msgs);
        this.$i18n.locale = locale;
      });
    }
  }
};
</script>

<style scoped>
.btn_no_effect {
  text-decoration: none;
  color: rgba(0, 0, 0, 0.87);
}
</style>
