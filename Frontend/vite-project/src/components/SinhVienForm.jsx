import { zodResolver } from "@hookform/resolvers/zod";
import { set, useForm } from "react-hook-form";
import { z } from "zod";

import { Button } from "@/components/ui/button";
import {
  Form,
  FormControl,
  FormDescription,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import { addSinhVien, updateSinhVien } from "@/api/api-sinhvien";
// import { useNavigate } from "react-router-dom";

const formSchema = z.object({
  ten: z.string().min(2, {
    message: "Ten must be at least 2 characters.",
  }),
  khoaId: z.coerce.number(
    {
      message: "Khoa Id must be a number",
    }
  ).min(1, {
    message: "Khoa Id must be at least 1 characters.",
  }),
  namBatDau: z.coerce.number(
    {
      message: "Nam Bat Dau must be a number",
    }
  ).min(4, {
    message: "Nam bat dau must be at least 4 characters.",
  })
  .refine((val) => val > 2010 && val < 2024, {
    message: "Trong so must be between 2010 and 2024",
  }),
});

export function SinhVienForm({ sinhVienId, handleAdd, handleEdit, setIsDialogOpen }) {
  // 1. Define your form.
  const form = useForm({
    resolver: zodResolver(formSchema),
    defaultValues: {
      id: sinhVienId,
      ten: "",
      khoaId: "",
      namBatDau: "",
    },
  });

  // 2. Define a submit handler.
  async function onSubmit(values) {
    // Do something with the form values.
    // ✅ This will be type-safe and validated.
    if (sinhVienId) {
      const data = await updateSinhVien(sinhVienId, values);
      handleEdit(data);
    } else {
      const data = await addSinhVien(values);
      handleAdd(data);
      setIsDialogOpen(false);
    }
  }

  return (
    <Form {...form}>
      <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-8">
        {sinhVienId && (
          <FormField
            control={form.control}
            name="id"
            render={({ field }) => (
              <FormItem>
                <FormLabel>Id</FormLabel>
                <FormControl>
                  <Input {...field} readOnly/>
                </FormControl>
                <FormDescription>
                  This is your unique identifier.
                </FormDescription>
                <FormMessage />
              </FormItem>
            )}
          />
        )}
        <FormField
          control={form.control}
          name="ten"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Tên</FormLabel>
              <FormControl>
                <Input placeholder="Nguyễn Văn A" {...field} />
              </FormControl>
              <FormDescription>
                This is your public display name.
              </FormDescription>
              <FormMessage />
            </FormItem>
          )}
        />
        <FormField
          control={form.control}
          name="khoaId"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Khoa Id</FormLabel>
              <FormControl>
                <Input placeholder="1" {...field} />
              </FormControl>
              <FormDescription>
                This is your public display name.
              </FormDescription>
              <FormMessage />
            </FormItem>
          )}
        />
        <FormField
          control={form.control}
          name="namBatDau"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Năm Bắt Đầu</FormLabel>
              <FormControl>
                <Input placeholder="2024" {...field} />
              </FormControl>
              <FormDescription>
                This is your public display name.
              </FormDescription>
              <FormMessage />
            </FormItem>
          )}
        />
        <Button type="submit">Submit</Button>
      </form>
    </Form>
  );
}
