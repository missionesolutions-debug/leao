export type Swagger = {
    "/api/v1/Authentication": {
        post: {
            params: {};
            requestBody: UsuarioLoginDto;
            result: null;
        };
    };
    "/api/v1/Users/me": {
        get: {
            params: {};
            requestBody: null;
            result: null;
        };
    };
    "/api/v1/blog/list": {
        get: {
            params: {
                category: string;
                search: string;
                tags: string;
                page: integer;
            };
            requestBody: null;
            result: null;
        };
    };
    "/api/v1/blog/detail/{url}": {
        get: {
            params: {
                url: string;
            };
            requestBody: null;
            result: null;
        };
    };
    "/api/v1/blog/destaques": {
        get: {
            params: {
                page: integer;
            };
            requestBody: null;
            result: null;
        };
    };
    "/api/v1/blog/categorias": {
        get: {
            params: {};
            requestBody: null;
            result: null;
        };
    };
    "/api/v1/configs": {
        get: {
            params: {};
            requestBody: null;
            result: null;
        };
    };
    "/api/workspaces/courses": {
        get: {
            params: {};
            requestBody: null;
            result: CoursesPage;
        };
    };
    "/api/v1/forms/contact": {
        post: {
            params: {};
            requestBody: ContactDTO;
            result: null;
        };
    };
    "/api/v1/forms/prelaunch": {
        post: {
            params: {};
            requestBody: User;
            result: null;
        };
    };
    "/api/v1/forms/work": {
        post: {
            params: {};
            requestBody: ContactDTO;
            result: null;
        };
    };
    "/api/v1/forms/newsletter": {
        post: {
            params: {};
            requestBody: ContactDTO;
            result: null;
        };
    };
    "/api/v1/language": {
        get: {
            params: {};
            requestBody: null;
            result: null;
        };
    };
    "/api/v1/word": {
        get: {
            params: {};
            requestBody: null;
            result: null;
        };
    };
    "/api/v1/metadata": {
        post: {
            params: {};
            requestBody: null;
            result: null;
        };
    };
    "/api/v1/metadata/{id}": {
        delete: {
            params: {
                id: string;
            };
            requestBody: null;
            result: null;
        };
    };
    "/OpenApi/generate": {
        get: {
            params: {};
            requestBody: null;
            result: null;
        };
    };
    "/api/v1/pages/{url}": {
        get: {
            params: {
                url: string;
            };
            requestBody: null;
            result: null;
        };
    };
    "/api/v1/pages/{entityKey}/listing": {
        get: {
            params: {
                entityKey: string;
                category: string;
                search: string;
                tags: string;
                page: integer;
                pageSize: integer;
            };
            requestBody: null;
            result: null;
        };
    };
    "/api/v1/pages/detail/{entityKey}/{url}": {
        get: {
            params: {
                entityKey: string;
                url: string;
            };
            requestBody: null;
            result: null;
        };
    };
    "/api/v1/section": {
        post: {
            params: {};
            requestBody: SectionDto;
            result: null;
        };
        get: {
            params: {
                page: string;
            };
            requestBody: null;
            result: null;
        };
    };
    "/api/v1/section/{id}": {
        put: {
            params: {
                id: integer;
            };
            requestBody: SectionDto;
            result: null;
        };
    };
    "/api/v1/Sitemap/xml": {
        get: {
            params: {};
            requestBody: null;
            result: null;
        };
    };
    "/api/v1/Sitemap/json": {
        get: {
            params: {};
            requestBody: null;
            result: null;
        };
    };
};
export interface Address {
    id?: any;
    ativo?: boolean;
    excluido?: boolean;
    dataCriacao?: string;
    dataEdicao?: string;
    userId?: any;
    fullAddress?: string;
    identification?: string;
    street?: string;
    number?: string;
    district?: string;
    complement?: string;
    zipCode?: string;
    city?: string;
    state?: string;
    mainAddress?: boolean;
    country?: string;
    cep?: string;
    localidade?: string;
    uf?: string;
    estado?: string;
    regiao?: string;
    ibge?: string;
    gia?: string;
    ddd?: string;
    siafi?: string;
}
export interface Categories {
    title?: string;
    courses?: CourseItemShort[];
}
export interface ContactDTO {
    name?: string;
    email?: string;
    phone?: string;
    subject?: string;
    message?: string;
    additionalFields?: Record<string, any>;
}
export interface Course {
    id?: any;
    guid?: string;
    title: string;
    description?: string;
    coverImageUrl?: string;
    url?: string;
    duration?: any;
    numberOfModules?: any;
    numberOfLessons?: any;
    numberOfReviews?: any;
    averageRating?: number;
    isActive?: boolean;
    isDeleted?: boolean;
    createdAt?: string;
    updatedAt?: string;
    updatedBy?: any;
    courseCategoryId?: any;
    courseCategory?: any;
    updatedByUser?: any;
    modules?: Module[];
    journeyCourses?: JourneyCourse[];
    userCourses?: UserCourse[];
}
export interface CourseCategory {
    id?: any;
    title: string;
    description?: string;
    isActive?: boolean;
    isDeleted?: boolean;
    createdAt?: string;
    updatedAt?: string;
    courses?: Course[];
}
export interface CourseItemShort {
    id?: any;
    title?: string;
    coverImageUrl?: string;
    duration?: any;
    averageRating?: number;
    url?: string;
    modules?: ModuleItemShort[];
    lessons?: LessonItemShort[];
}
export interface CoursesPage {
    banners?: Item[];
    categories?: Categories[];
}
export interface Item {
    id?: any;
    ordem?: any;
    titulo?: string;
    subtitulo?: string;
    thumbnail?: string;
    imagem?: string;
    ref?: string;
    pageTitle?: string;
    metaDescription?: string;
    imageOpenGraph?: string;
    headScripts?: string;
    bodyScripts?: string;
    groupPagina?: string;
    imagemAlt?: string;
    thumbnailAlt?: string;
    imagemMobile?: string;
    arquivo?: string;
    url?: string;
    tags?: string;
    destaque?: boolean;
    datas?: string;
    menu?: boolean;
    data?: string;
    dataCriacao?: string;
    link?: string;
    descricao?: string;
    dataCadastro?: string;
    fields?: Record<string, any>;
    imagens?: Item[];
    items?: Item[];
}
export interface Journey {
    id?: any;
    userId?: any;
    title: string;
    description?: string;
    startDate?: string;
    endDate?: string;
    status?: string;
    progressData?: string;
    isActive?: boolean;
    isDeleted?: boolean;
    createdAt?: string;
    updatedAt?: string;
    user?: any;
    journeyCourses?: JourneyCourse[];
}
export interface JourneyCourse {
    journeyId?: any;
    courseId?: any;
    journey?: any;
    course?: any;
}
export interface Lesson {
    id?: any;
    guid?: string;
    moduleId?: any;
    title: string;
    contentUrl?: string;
    duration?: any;
    order?: any;
    isActive?: boolean;
    isDeleted?: boolean;
    createdAt?: string;
    updatedAt?: string;
    thumbnail?: string;
    url?: string;
    videoContent?: string;
    description?: string;
    isFeatured?: boolean;
    updatedBy?: any;
    module?: any;
    updatedByUser?: any;
}
export interface LessonItemShort {
    id?: any;
    title?: string;
    order?: any;
    thumbnail?: string;
    url?: string;
    videoContent?: string;
    description?: string;
    isFeatured?: boolean;
}
export interface Module {
    id?: any;
    guid?: string;
    courseId?: any;
    title: string;
    description?: string;
    coverImageUrl?: string;
    duration?: any;
    numberOfLessons?: any;
    order?: any;
    isActive?: boolean;
    isDeleted?: boolean;
    createdAt?: string;
    updatedAt?: string;
    updatedBy?: any;
    course?: any;
    updatedByUser?: any;
    lessons?: Lesson[];
}
export interface ModuleItemShort {
    id?: any;
    title?: string;
    order?: any;
    duration?: any;
    lessons?: LessonItemShort[];
}
export interface Permission {
    id?: any;
    name: string;
    description?: string;
    isActive?: boolean;
    isDeleted?: boolean;
    createdAt?: string;
    updatedAt?: string;
    rolePermissions?: RolePermission[];
    subscriptionPlanPermissions?: SubscriptionPlanPermission[];
}
export interface Role {
    id?: any;
    name: string;
    description?: string;
    isActive?: boolean;
    isDeleted?: boolean;
    createdAt?: string;
    updatedAt?: string;
    userRoles?: UserRole[];
    rolePermissions?: RolePermission[];
}
export interface RolePermission {
    roleId?: any;
    permissionId?: any;
    role?: any;
    permission?: any;
}
export interface SectionDto {
    id?: any;
    ref?: string;
    linkUrl?: string;
    videoUrl?: string;
    jsonContent?: string;
    enabled?: boolean;
    i18n?: Record<string, any>;
}
export interface SubscriptionPlan {
    id?: any;
    guid?: string;
    planName: string;
    description?: string;
    price?: number;
    durationInDays?: any;
    isActive?: boolean;
    isDeleted?: boolean;
    createdAt?: string;
    updatedAt?: string;
    subscriptionPlanPermissions?: SubscriptionPlanPermission[];
    userSubscriptions?: UserSubscription[];
}
export interface SubscriptionPlanPermission {
    subscriptionPlanId?: any;
    permissionId?: any;
    subscriptionPlan?: any;
    permission?: any;
}
export interface TranslationDto {
    title?: string;
    subtitle?: string;
    description?: string;
    linkText?: string;
}
export interface User {
    id?: any;
    ativo?: boolean;
    excluido?: boolean;
    dataCriacao?: string;
    dataEdicao?: string;
    email?: string;
    username?: string;
    password?: string;
    role?: string;
    avatar?: string;
    name?: string;
    surname?: string;
    guid?: string;
    birthday?: string;
    gender?: string;
    phone?: string;
    cpf?: string;
    address?: any;
    userRoles?: UserRole[];
    userSubscriptions?: UserSubscription[];
    userCourses?: UserCourse[];
    journeys?: Journey[];
}
export interface UserCourse {
    id?: any;
    userId?: any;
    courseId?: any;
    purchaseDate?: string;
    accessStartDate?: string;
    accessEndDate?: string;
    isActive?: boolean;
    isDeleted?: boolean;
    user?: any;
    course?: any;
}
export interface UserRole {
    userId?: any;
    roleId?: any;
    user?: any;
    role?: any;
}
export interface UserSubscription {
    id?: any;
    userId?: any;
    subscriptionPlanId?: any;
    startDate?: string;
    endDate?: string;
    isActive?: boolean;
    isDeleted?: boolean;
    createdAt?: string;
    updatedAt?: string;
    user?: any;
    subscriptionPlan?: any;
}
export interface UsuarioLoginDto {
    emailAddress?: string;
    password?: string;
}
