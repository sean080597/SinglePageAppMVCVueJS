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
        <v-text-field
          label="svserverpath"
          placeholder="SVServer Path"
          v-model="genSVServerPath"
          solo
          disabled
        ></v-text-field>
      </v-flex>
      <!-- Import Button -->
      <v-btn color="primary" light class="mb-2" :disabled="!calcIsChecked" @click="ImportAndDel">Import</v-btn>
    </v-toolbar>
    <v-data-table
      :headers="headers"
      :items="ls_allfiles"
      v-model="selected_import"
      select-all
      item-key="GroupNumber"
      class="elevation-1"
      :loading="!isLoadedData"
    >
      <v-progress-linear v-slot:progress color="blue" indeterminate></v-progress-linear>
      <template v-slot:items="props">
        <td>
          <v-checkbox
            v-model="props.selected"
            primary
            hide-details
            :disabled="genStatusShow(props.item) == 'Error' || genStatusShow(props.item) == 'Invalid'"
          ></v-checkbox>
        </td>
        <td class="text-xs-left">{{ props.item.BaseCode }}</td>
        <td class="text-xs-left">{{ props.item.GroupNumber }}</td>
        <td class="text-xs-left">{{ props.item.Senddate | dateReformat}}</td>
        <td
          class="text-xs-left"
        >{{ props.item.FileOwner.FileOwnNames[0].Fm + ' ' + props.item.FileOwner.FileOwnNames[0].Gv}}</td>
        <td class="text-xs-left">{{ props.item.BankcdShow }}</td>
        <td class="text-xs-left">{{ caclRecruiterCode(props.item) }}</td>
        <td class="text-xs-right">{{ props.item.Life_hosts.length }}</td>
        <td
          class="text-xs-left"
        >{{ props.item.Life_hosts[props.item.Life_hosts.length - 1].Cov[0].Crtable }}</td>
        <td class="text-xs-left">
          <v-subheader :class="genSttColor(props.item)">{{genStatusShow(props.item)}}</v-subheader>
          <!-- <v-icon :class="genSttColor(props.item)">{{genStatusShow(props.item)}}</v-icon> -->
        </td>
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
    isLoadedData: false,
    headers: [
      { text: "BaseCode", value: "BaseCode", sortable: false },
      { text: "GroupNumber", value: "GroupNumber", sortable: false },
      { text: "GroupSendDate", value: "Senddate", sortable: false },
      { text: "SubcriberName", value: "SubcriberName", sortable: false },
      { text: "BankcdShow", value: "BankcdShow", sortable: false },
      { text: "RecruiterCode", value: "RecruiterCode", sortable: false },
      { text: "UpCntShow", value: "UpCntShow", sortable: false },
      { text: "CrtableShow", value: "CrtableShow", sortable: false },
      { text: "StatusShow", value: "StatusShow", sortable: false }
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
    genSVServerPath() {
      return this.isObject(this.svserver_path)
        ? this.svserver_path.PATH
        : this.svserver_path;
    },
    calcBcode() {
      if (this.isObject(this.svserver_path)) {
        return this.svserver_path.DIRECTORY;
      }
      let arr = this.svserver_path.split("\\");
      return arr[arr.length - 1];
    }
  },

  created() {
    this.loadSVServer();
    Fire.$on("ReloadTable", () => {
      this.loadAllFiles(this.genSVServerPath)
    });
  },

  methods: {
    isObject(a) {
      return !!a && a.constructor === Object;
    },
    loadAllFiles(filePath) {
      this.isLoadedData = false;
      axios
        .get("/api/svserver/getFilesContent?filePath=" + filePath)
        .then(({ data }) => {
          this.ls_allfiles = data;
          this.isLoadedData = true;
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
    caclRecruiterCode(file) {
      return file.Agt_no != null &&
        file.Agt_no.length > 1 &&
        file.Agt_no[0] != null
        ? file.Agt_no[0]
        : file.BankPerson;
    },
    genStatusShow(file) {
      if (file.File_ver != "9" && file.File_ver != "7") return "Error";
      if (file.GroupNumber == null || file.Bcode != this.calcBcode)
        return "Invalid";
      return "In progress";
    },
    genSttColor(file) {
      if (
        this.genStatusShow(file) == "Error" ||
        this.genStatusShow(file) == "Invalid"
      )
        return "red--text text--darken-1";
      return "blue--text text--lighten-1";
    },
    ImportAndDel() {
      let dtime = Vue.filter("dateYMDHMS")(new Date());
      let svserver_path = this.genSVServerPath;
      $.each(this.selected_import, function(indexInArray, valueOfElement) {
        axios.post("/api/svserver/insertPRGR", valueOfElement).then(({data}) => {
          let res = JSON.parse(data)
          toast.fire({
            type: res.type,
            title: res.value
          });
          //reload
          Fire.$emit("ReloadTable");
        });
      });
    }
  }
};
</script>