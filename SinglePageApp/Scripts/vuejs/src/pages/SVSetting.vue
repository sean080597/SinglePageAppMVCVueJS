<template>
  <div>
    <v-data-table :headers="headers" :items="desserts" :rows-per-page-items="[10, 20, 30]" :loading="!isLoadedData">
      <v-progress-linear v-slot:progress color="blue" indeterminate></v-progress-linear>
      <template v-slot:items="props">
        <td>
          {{ props.item.SETTINGNAME }}
        </td>
        <td class="text-xs-left">
            <v-edit-dialog
            :return-value.sync="props.item.SETTINGVALUE"
            lazy
            large
            persistent
            @save="save(props.item.SETTINGNAME, props.item.SETTINGVALUE)"
          >
            {{ props.item.SETTINGVALUE }}
            <template v-slot:input>
              <v-text-field
                v-model="props.item.SETTINGVALUE"
                :rules="[max25chars]"
                counter
              ></v-text-field>
            </template>
          </v-edit-dialog>
        </td>
        <td class="text-xs-left">{{ props.item.ISEXPIRED }}</td>
        <td class="text-xs-left">{{ props.item.DESCRIPTION }}</td>
      </template>
    </v-data-table>
  </div>
</template>

<script>
export default {
  data() {
    return {
      isLoadedData: false,
      max25chars: v => v.length <= 25 || "Input too long!",
      headers: [
        {
          text: "Setting Name",
          align: "left",
          sortable: false,
          value: "SETTINGNAME"
        },
        { text: "Setting Value", value: "SETTINGVALUE", sortable: false },
        { text: "Is Expired", value: "ISEXPIRED", sortable: false },
        { text: "DESCRIPTION", value: "DESCRIPTION", sortable: false }
      ],
      desserts: []
    };
  },
  methods: {
    loadSettings() {
      this.isLoadedData = false
      axios.get("/api/svserver/settings").then(({ data }) => {
        this.desserts = data
        this.isLoadedData = true
      });
    },
    save(setting_name, setting_value) {
      axios.post('/api/svserver/changeSettingValue?key=' + setting_name + '&value=' + setting_value)
      .then(() => {
        //reload
        Fire.$emit("ReloadSettings");
        toast.fire({
          type: "success",
          title: "Changed setting successfully"
        })
      })
      .catch(error => console.log(error.response.data))
    }
  },
  created(){
    this.loadSettings()
    Fire.$on("ReloadSettings", () => {
      this.loadSettings();
    });
  }
};
</script>