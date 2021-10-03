<template>
  <v-card class="px-3 pt-0" flat>
    <v-card class="d-inline-block elevation-12">
      <v-navigation-drawer floating light permanent value="true" class="grey lighten-3">
        <v-list>
          <v-list-tile :to="linkToCreateOrEdit">
            <v-list-tile-action>
              <v-icon class="grey--text text--darken-4">create</v-icon>
            </v-list-tile-action>

            <v-list-tile-content>
              <v-list-tile-title class="grey--text text--darken-4">Create {{cate}}</v-list-tile-title>
            </v-list-tile-content>
          </v-list-tile>

          <v-list-tile @click="delSelected" :disabled="!value">
            <v-list-tile-action>
              <v-icon :class="!value?'grey--text text--lighten-1':'grey--text text--darken-4'">delete</v-icon>
            </v-list-tile-action>

            <v-list-tile-content>
              <v-list-tile-title
                :class="!value?'grey--text text--lighten-1':'grey--text text--darken-4'"
              >Delete {{cate}}</v-list-tile-title>
            </v-list-tile-content>
          </v-list-tile>
        </v-list>
      </v-navigation-drawer>
    </v-card>
  </v-card>
</template>

<script>
export default {
  props: ["cate", "value"],
  methods: {
    delSelected() {
      this.$emit('delSelected')
    }
  },
  computed: {
    linkToCreateOrEdit() {
      return "/" + this.cate + "/create";
    }
  }
};
</script>