from datetime import datetime
from typing import List, Optional
from sqlalchemy.orm import Session, joinedload
from models import Course, Module, ModuleLesson, Lesson, CourseCategory, Arquivo  # Ajuste com seus models
from schemas import (
    CoursesPage, CourseItemShort, ModuleItemShort, LessonItemShort,
    LessonDetailPage, ModuleDetailPage, CourseItemShortless, CourseItemMin, LessonFile
)

class CourseService:
    def __init__(self, db: Session):
        self.db = db

    # --- CRUD de Curso ---
    def save_or_update(self, course: Course) -> Course:
        if course.id is None:
            self.db.add(course)
        else:
            self.db.merge(course)
        self.db.commit()
        return course

    # --- Página de Cursos ---
    def get_courses_page(self, is_authenticated: bool) -> CoursesPage:
        courses_page = CoursesPage()
        
        # Obter cursos e categorias
        course_categories = (
            self.db.query(CourseCategory)
            .options(joinedload(CourseCategory.courses)
                     .joinedload(Course.course_modules)
                     .joinedload(lambda cm: cm.module)
                     .joinedload(Module.module_lessons)
                     .joinedload(lambda ml: ml.lesson))
            .filter(CourseCategory.is_active == True)
            .all()
        )

        for course_category in course_categories:
            courses = []
            for course in course_category.courses:
                if not course.is_active or course.is_deleted:
                    continue
                
                modules = []
                for cm in course.course_modules:
                    if not cm.module.is_active or cm.module.is_deleted or not cm.destaque_vitrine:
                        continue
                    
                    lessons = []
                    for ml in cm.module.module_lessons:
                        if not ml.lesson.is_active or ml.lesson.is_deleted:
                            continue
                        lesson = LessonItemShort(
                            id=ml.lesson.id,
                            title=ml.lesson.title,
                            duration=ml.lesson.duration,
                            order=ml.lesson.ordem,
                            description=ml.lesson.description,
                            is_featured=ml.lesson.is_featured or False,
                            thumbnail=ml.lesson.thumbnail,
                            url=ml.lesson.guid.hex if is_authenticated else None,
                            video_content=ml.lesson.video_content if is_authenticated else None
                        )
                        lessons.append(lesson)
                    
                    total_lesson_duration = sum(l.duration or 0 for l in lessons)
                    modules.append(ModuleItemShort(
                        id=cm.module.id,
                        title=cm.module.title,
                        duration=self.format_duration(total_lesson_duration),
                        total_minutes=total_lesson_duration,
                        thumbnail=cm.module.thumbnail,
                        type="Module",
                        order=cm.module.ordem,
                        number_of_lessons=len(lessons),
                        lessons=sorted(lessons, key=lambda x: x.order)
                    ))
                
                total_module_duration = sum(m.total_minutes for m in modules)
                courses.append(CourseItemShort(
                    id=course.id,
                    title=course.title,
                    cover_image_url=course.cover_image_url,
                    duration=self.format_duration(total_module_duration),
                    total_minutes=total_module_duration,
                    average_rating=course.average_rating,
                    url=course.guid.hex,
                    number_of_lessons=sum(m.number_of_lessons for m in modules),
                    modules=sorted(modules, key=lambda x: x.order)
                ))
            
            courses_page.categories.append({
                "title": course_category.title,
                "total_minutes": sum(c.total_minutes for c in courses),
                "number_of_lessons": sum(c.number_of_lessons for c in courses),
                "courses": courses
            })

        return courses_page

    def get_course_page(self, guid: str, is_authenticated: bool) -> CourseItemShort:
        course = self.db.query(Course).filter(Course.guid == guid).first()
        if not course:
            return None
        
        modules = []
        for cm in course.course_modules:
            lessons = []
            for ml in cm.module.module_lessons:
                l = ml.lesson
                lessons.append(LessonItemShort(
                    id=l.id,
                    title=l.title,
                    duration=l.duration,
                    order=l.ordem,
                    description=l.description,
                    is_featured=l.is_featured or False,
                    thumbnail=l.thumbnail,
                    url=l.guid.hex if is_authenticated else None,
                    video_content=l.video_content if is_authenticated else None
                ))
            
            total_lesson_duration = sum(l.duration or 0 for l in lessons)
            modules.append(ModuleItemShort(
                id=cm.module.id,
                title=cm.module.title,
                duration=self.format_duration(total_lesson_duration),
                total_minutes=total_lesson_duration,
                thumbnail=cm.module.thumbnail,
                type="Module",
                order=cm.module.ordem,
                number_of_lessons=len(lessons),
                lessons=sorted(lessons, key=lambda x: x.order)
            ))

        total_module_duration = sum(m.total_minutes for m in modules)
        return CourseItemShort(
            id=course.id,
            title=course.title,
            cover_image_url=course.cover_image_url,
            duration=self.format_duration(total_module_duration),
            total_minutes=total_module_duration,
            average_rating=course.average_rating,
            url=course.guid.hex,
            number_of_lessons=sum(m.number_of_lessons for m in modules),
            modules=sorted(modules, key=lambda x: x.order)
        )

    def get_lesson_page(self, guid: str, is_authenticated: bool) -> LessonDetailPage:
        lesson = self.db.query(Lesson).filter(Lesson.guid == guid).first()
        if not lesson:
            return None
        
        lesson_files = self.db.query(Arquivo).filter(
            Arquivo.table_action == "Lessons",
            Arquivo.table_id == lesson.id
        ).all()
        
        lesson_files_dto = [
            LessonFile(
                file_name=f.file_name,
                file_source=f.file_data,
                file_image=f.file_image,
                file_description=f.description
            ) for f in lesson_files
        ]
        
        return LessonDetailPage(
            page=LessonItemShort(
                id=lesson.id,
                title=lesson.title,
                order=lesson.ordem,
                description=lesson.description,
                is_featured=lesson.is_featured or False,
                thumbnail=lesson.thumbnail,
                url=lesson.guid.hex if is_authenticated else None,
                video_content=lesson.video_content if is_authenticated else None,
                lesson_files=lesson_files_dto
            )
        )

    @staticmethod
    def format_duration(total_minutes: int) -> str:
        hours, minutes = divmod(total_minutes, 60)
        if hours > 0:
            return f"{hours}h{minutes}min" if minutes > 0 else f"{hours}h"
        return f"{minutes}min"
