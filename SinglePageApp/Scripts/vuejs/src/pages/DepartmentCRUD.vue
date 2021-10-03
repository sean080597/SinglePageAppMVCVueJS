<template>
  <div>
    <v-toolbar flat color="white">
      <v-toolbar-title>My Department CRUD</v-toolbar-title>
      <v-divider class="mx-2" inset vertical></v-divider>
      <!-- New Item Button -->
      <v-dialog v-model="dialog" max-width="500px">
        <template v-slot:activator="{ on }">
          <v-btn color="primary" dark class="mb-2" v-on="on">New Item</v-btn>
        </template>
        <v-card>
          <v-card-title>
            <span class="headline">{{ formTitle }}</span>
          </v-card-title>

          <v-card-text>
            <v-container grid-list-md>
              <v-layout wrap>
                <v-flex xs12 sm5>
                  <v-text-field
                    v-model="editedItem.DEPARTMENT_NAME"
                    label="Department Name"
                    v-validate="'required|max:50'"
                    :error-messages="errors.collect('Department Name')"
                    data-vv-name="Department Name"
                    required
                  ></v-text-field>
                </v-flex>
                <v-flex xs12 sm5 offset-sm1>
                  <v-text-field
                    v-model="editedItem.DEPARTMENT_HEAD"
                    label="Department Head"
                    v-validate="'required|max:30'"
                    :error-messages="errors.collect('Department Head')"
                    data-vv-name="Department Head"
                    required
                  ></v-text-field>
                </v-flex>
              </v-layout>
            </v-container>
          </v-card-text>

          <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn color="blue darken-1" flat @click="close">Cancel</v-btn>
            <v-btn color="blue darken-1" flat @click="save">Save</v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>

      <!-- Delete Button -->
      <v-btn
        color="error"
        class="mb-2"
        :disabled="!calcIsChecked"
        @click="callDeleteSelected"
      >Delete</v-btn>
    </v-toolbar>
    <v-data-table
      :headers="headers"
      :items="departments"
      v-model="selected"
      select-all
      item-key="DEPARTMENT_ID"
      class="elevation-1"
    >
      <template v-slot:items="props">
        <td>
          <v-checkbox v-model="props.selected" primary hide-details></v-checkbox>
        </td>
        <td class="text-xs-left">{{ props.item.DEPARTMENT_NAME }}</td>
        <td class="text-xs-left">{{ props.item.DEPARTMENT_HEAD }}</td>
        <td class="layout px-4">
          <v-icon small class="mr-2" @click="editItem(props.item)">edit</v-icon>
          <v-icon small @click="callDeleteSingle(props.item)">delete</v-icon>
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
  $_veeValidate: {
    validator: "new"
  },
  data: () => ({
    dialog: false,
    headers: [
      { text: "Department Name", value: "DEPARTMENT_NAME" },
      { text: "Department Head", value: "DEPARTMENT_HEAD" },
      { text: "Actions", value: "name", sortable: false }
    ],
    selected: [],
    departments: [],
    editedIndex: -1,
    editedItem: {
      DEPARTMENT_ID: "",
      DEPARTMENT_NAME: "",
      DEPARTMENT_HEAD: ""
    },
    defaultItem: {
      DEPARTMENT_ID: "",
      DEPARTMENT_NAME: "",
      DEPARTMENT_HEAD: ""
    },
    dictionary: {
      custom: {
        "Department Name": {
          required: () => "Name can not be empty",
          max: "The name field may not be greater than 50 characters"
        }
      }
    }
  }),

  computed: {
    formTitle() {
      return this.editedIndex === -1 ? "New Item" : "Edit Item";
    },
    calcIsChecked() {
      return this.selected.length > 0 ? true : false;
    }
  },

  watch: {
    dialog(val) {
      val || this.close();
    }
  },

  created() {
    this.initialize();
    Fire.$on("ReloadDepartments", () => {
      this.initialize();
    });
  },

  mounted() {
    this.$validator.localize("en", this.dictionary);
  },

  methods: {
    loadDepartments() {
      this.$Progress.start();
      axios
        .get("/api/department")
        .then(
          ({ data }) => ((this.departments = data), this.$Progress.finish())
        );
    },

    initialize() {
      this.loadDepartments();
    },

    editItem(item) {
      this.editedIndex = this.departments.indexOf(item);
      this.editedItem = Object.assign({}, item);
      this.dialog = true;
    },

    // Handle Delete ============================================================
    deleteItem(departID) {
      this.$Progress.start();
      axios
        .delete("/api/department/" + departID)
        .then(() => {
          //notify
          Swal.fire("Deleted!", "Deleted successfully.", "success");
          this.$Progress.finish();
          //reload departments
          Fire.$emit("ReloadDepartments");
        })
        .catch(() => {
          Swal.fire("Failed!", "Some Error Occurred!", "warning");
          this.$Progress.fail();
        });
    },

    callDeleteSingle(item) {
      Swal.fire({
        title: "Do you wanna delete this records?",
        text: "Cannot restore this records!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Delete!"
      }).then(result => {
        if (result.value) {
          this.deleteItem(item.DEPARTMENT_ID);
        }
      });
    },

    callDeleteSelected() {
      Swal.fire({
        title: "Do you wanna delete these records?",
        text: "Cannot restore these records!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Delete!"
      }).then(result => {
        this.selected.forEach(element => {
          this.deleteItem(element.DEPARTMENT_ID);
        });
      });
    },

    // =========================================================================
    close() {
      this.dialog = false;
      setTimeout(() => {
        this.editedItem = Object.assign({}, this.defaultItem);
        this.editedIndex = -1;
      }, 300);
    },

    // Handle saving data
    save() {
      this.$Progress.start();
      //request to server
      this.$validator.validateAll().then(result => {
        if (result) {
          if (this.editedIndex > -1) {
            axios
              .put(
                "/api/department/" + this.editedItem.DEPARTMENT_ID,
                this.editedItem
              )
              .then(res => {
                toast.fire({
                  type: "success",
                  title: "Edited Department successfully"
                });

                //reload departments
                Fire.$emit("ReloadDepartments");

                this.$Progress.finish();
                this.close();
              })
              .catch(() => {
                Swal("Failed!", "Some error occurred", "warning");
                this.$Progress.fail();
              });
          } else {
            axios
              .post("/api/department", {
                DEPARTMENT_NAME: this.editedItem.DEPARTMENT_NAME,
                DEPARTMENT_HEAD: this.editedItem.DEPARTMENT_HEAD
              })
              .then(res => {
                toast.fire({
                  type: "success",
                  title: "Created Department successfully"
                });

                //reload departments
                Fire.$emit("ReloadDepartments");

                this.$Progress.finish();
                this.close();
              })
              .catch(() => {
                Swal("Failed!", "Some error occurred", "warning");
                this.$Progress.fail();
              });
          }
        } else {
          this.$Progress.fail();
        }
      });
    }
  }
};
</script>