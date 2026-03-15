import { createRouter, createWebHistory } from 'vue-router';

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      redirect: '/applications',
    },
    {
      path: '/applications',
      name: 'applications',
      component: () => import('@/views/JobApplicationsListView.vue'),
    },
    {
      path: '/applications/create',
      name: 'application-create',
      component: () => import('@/views/JobApplicationCreateView.vue'),
    },
    {
      path: '/applications/:id',
      name: 'application-details',
      component: () => import('@/views/JobApplicationDetailsView.vue'),
      props: true,
    },
    {
      path: '/applications/:id/edit',
      name: 'application-edit',
      component: () => import('@/views/JobApplicationEditView.vue'),
      props: true,
    },
  ],
});

export default router;
