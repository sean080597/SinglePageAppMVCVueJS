<template>
  <div>
    <v-toolbar flat color="white">
      <v-toolbar-title>SVSERVER</v-toolbar-title>
      <v-divider class="mx-2" inset vertical></v-divider>
      <v-flex sm3>
        <v-autocomplete
          :items="ls_svserver"
          item-text="DESCRIPTION"
          item-value="PATH"
          label="SVServer Name"
          v-model="svserver_path"
          @change="changeSelected()"
        ></v-autocomplete>
      </v-flex>
      <v-divider class="mx-2" inset vertical></v-divider>
      <v-flex sm3>
        <v-text-field label="svserverpath" placeholder="SVServer Path" v-model="genSVServerPath" solo disabled></v-text-field>
      </v-flex>
      <!-- Import Button -->
      <v-btn color="primary" light class="mb-2" :disabled="!calcIsChecked" @click="ImpAndLog">Import</v-btn>
    </v-toolbar>
    <v-data-table
      :headers="headers"
      :items="ls_allfiles"
      v-model="selected_import"
      select-all
      item-key="FileName"
      class="elevation-1"
    >
      <template v-slot:items="props">
        <td>
          <v-checkbox v-model="props.selected" primary hide-details></v-checkbox>
        </td>
        <td class="text-xs-left">{{ props.item.FileName }}</td>
        <td class="text-xs-left">{{ props.item.FilePath }}</td>
        <td class="text-xs-left">{{ props.item.FileDateModified | dateYMD }}</td>
      </template>
      <template v-slot:no-data>
        <v-alert :value="true" color="error" icon="warning">No data to show!</v-alert>
      </template>
    </v-data-table>
  </div>
</template>

<script>
export default {
  data: () => ({
    headers: [
      { text: "Name", value: "FileName" },
      { text: "Path", value: "FilePath", sortable: false },
      { text: "Date Modified", value: "FileDateModified" }
    ],
    ls_svserver: [],
    svserver_path: "",

    ls_allfiles: [],
    selected_import: []
  }),

  computed: {
    calcIsChecked() {
      return this.selected_import.length > 0 ? true : false;
    },
    genSVServerPath(){
      return this.isObject(this.svserver_path) ? this.svserver_path.PATH : this.svserver_path
    }
  },

  created() {
    this.loadSVServer();
    Fire.$on("ReloadTable", () => {
      //
    });
  },

  methods: {
    isObject(a) {
      return (!!a) && (a.constructor === Object);
    },
    loadAllFiles(filePath) {
      axios
        .get("/api/svserver/getFiles?filePath=" + filePath)
        .then(({ data }) => {
          this.ls_allfiles = data;
        });
    },
    loadSVServer() {
      this.$Progress.start();
      axios
        .get("/api/svserver")
        .then(
          ({ data }) => (
            (this.ls_svserver = data),
            (this.svserver_path = data[0]),
            this.loadAllFiles(data[0]["PATH"]),
            this.$Progress.finish()
          )
        );
    },
    changeSelected() {
      this.loadAllFiles(this.svserver_path);
    },
    ImpAndLog() {
      let dtime = Vue.filter("dateYMDHMS")(new Date())
      let svserver_path = this.genSVServerPath
      $.each(this.selected_import, function (indexInArray, valueOfElement) {
        axios.post('/api/svserver/writeLogFile?logPath=' + svserver_path + '&datetime=' + dtime, valueOfElement)
        .then(() => {
          toast.fire({
            type: "success",
            title: "Written log file successfully"
          });
      });
      })
    }
  }
};
</script>