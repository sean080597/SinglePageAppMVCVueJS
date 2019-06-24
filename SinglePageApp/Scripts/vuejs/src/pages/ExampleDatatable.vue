<template>
  <v-form>
    <v-container>
      <v-layout wrap>
        <v-flex xs12>
          <v-autocomplete
            v-model="lang"
            :items="languages"
            box
            chips
            color="blue-grey lighten-2"
            item-text="name"
            item-value="name"
          >
            <template v-slot:selection="data">
              <v-chip
                :selected="data.selected"
                class="chip--select-multi"
              >
                <v-avatar>
                  <span class="flag-icon flag-icon-gr flag-icon-squared"></span>
                </v-avatar>
                {{ data.item.name }}
              </v-chip>
            </template>
            <template v-slot:item="data">
              <template v-if="typeof data.item !== 'object'">
                <v-list-tile-content v-text="data.item"></v-list-tile-content>
              </template>
              <template v-else>
                <v-list-tile-avatar>
                  <span class="flag-icon flag-icon-gr flag-icon-squared"></span>
                </v-list-tile-avatar>
                <v-list-tile-content>
                  <v-list-tile-title v-html="data.item.name"></v-list-tile-title>
                </v-list-tile-content>
              </template>
            </template>
          </v-autocomplete>
        </v-flex>
      </v-layout>
    </v-container>
  </v-form>
</template>

<script>
  export default {
    data () {
      const srcs = {
        1: 'https://cdn.vuetifyjs.com/images/lists/1.jpg',
        2: 'https://cdn.vuetifyjs.com/images/lists/2.jpg',
        3: 'https://cdn.vuetifyjs.com/images/lists/3.jpg',
        4: 'https://cdn.vuetifyjs.com/images/lists/4.jpg',
        5: 'https://cdn.vuetifyjs.com/images/lists/5.jpg'
      }

      return {
        autoUpdate: true,
        lang: 'English',
        languages: [
          { name: 'English', icon: 'us' },
          { name: 'Japanese', icon: 'ja' },
        ],
      }
    },

    methods: {
      remove (item) {
        const index = this.friends.indexOf(item.name)
        if (index >= 0) this.friends.splice(index, 1)
      }
    }
  }
</script>