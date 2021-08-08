<template>
  <loading-overlay v-if="loading" message="Loading"></loading-overlay>

  <div ref="modal" class="modal fade" tabindex="-1" id="blockUserModal">
    <div class="modal-dialog">
      <form class="modal-content" @submit="blockUserSubmit" :class="{ 'was-validated': blockValidated }" novalidate>
        <div class="modal-header">
          <h5 class="modal-title">Block User</h5>
          <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
        </div>
        <div class="modal-body">
          <div class="mb-3">
            <label for="blockUsername" class="form-label">Username</label>
            <input v-model="newBlockUsername" class="form-control" id="blockUsername" type="text" required :disabled="blockLoading"  />
          </div>
          <span v-if="blockError" class="text-danger">{{blockError}}</span>
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-secondary" data-bs-dismiss="modal" :disabled="blockLoading">Cancel</button>
          <button type="submit" class="btn btn-danger" :disabled="blockLoading">
            <span class="spinner-border spinner-border-sm" v-if="blockLoading"></span>
            Block
          </button>
        </div>
      </form>
    </div>
  </div>

  <h1>Blocked Users</h1>
  <ul class="list-group mb-3">
    <li class="list-group-item d-flex justify-content-between align-items-start" v-if="!blocks.length">
      <div class="text-muted">There are no blocked users</div>
    </li>
    <li class="list-group-item d-flex justify-content-between align-items-start" v-for="block in blocks" :key="block.blockedUserId">
      <div class="ms-2 me-auto">
        <div class="fw-bold">{{block.blockedUsername}}</div>
        <a class="link-success" @click="unblock(block.blockedUserId)">Unblock</a>
      </div>
      <span class="badge bg-danger rounded-pill">BLOCKED</span>
    </li>
  </ul>
  <button class="btn btn-danger" data-bs-toggle="modal" data-bs-target="#blockUserModal" @click="initBlock">Block a User</button>
</template>

<style lang="scss">

</style>

<script lang="ts">
import { Options } from 'vue-class-component';
import { Block, BlockService } from '../services/blockService';
import LoadingOverlay from '../components/LoadingOverlay.vue';
import AuthenticatedVue from '@/components/AuthenticatedVue';
import $ from 'jquery';
import 'bootstrap';

@Options({
  components: {
    LoadingOverlay
  }
})
export default class Blocks extends AuthenticatedVue {
  public loading = false;
  public blocks: Block[] = [];
  public blockValidated = false;
  public blockLoading = false;
  public newBlockUsername = '';
  public blockError = '';

  public async beforeMount (): Promise<void> {
    this.loading = true;
    this.blocks = await BlockService.get();
    this.loading = false;
  }

  public initBlock (): void {
    this.newBlockUsername = '';
    this.blockError = '';
    this.blockValidated = false;
  }

  public async unblock (userId: string): Promise<void> {
    this.loading = true;
    await BlockService.delete(userId);
    this.blocks = await BlockService.get();
    this.loading = false;
  }

  public async blockUserSubmit (e: Event): Promise<void> {
    e.preventDefault();
    const target = e.target as HTMLFormElement;
    this.blockValidated = true;
    if (target.reportValidity()) {
      this.blockValidated = false;
      if (this.newBlockUsername === this.username) {
        this.blockError = 'You cannot block yourself';
        return;
      }

      try {
        this.blockLoading = true;
        await BlockService.create(this.newBlockUsername);
        this.blocks = await BlockService.get();
        $('#blockUserModal').modal('hide');
      } catch (e) {
        if (e?.response?.status === 404) {
          this.blockError = 'User does not exist';
        } else if (e?.response?.status === 409) {
          this.blockError = 'User is already blocked';
        } else {
          this.blockError = 'An error occurred';
        }
      } finally {
        this.blockLoading = false;
      }
    }
  }
}
</script>
