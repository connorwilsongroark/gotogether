import type { ButtonHTMLAttributes } from "react";
import { cloneElement, isValidElement } from "react";

type ButtonVariant = "primary" | "secondary" | "ghost";
type ButtonSize = "sm" | "md";

interface ButtonProps extends ButtonHTMLAttributes<HTMLButtonElement> {
  variant?: ButtonVariant;
  size?: ButtonSize;
  asChild?: boolean;
}

export default function Button({
  variant = "secondary",
  size = "md",
  asChild = false,
  className = "",
  children,
  ...props
}: ButtonProps) {
  const base =
    "inline-flex items-center justify-center rounded-md font-medium " +
    "transition-colors focus:outline-none focus:ring-2 focus:ring-brand";

  const variants: Record<ButtonVariant, string> = {
    primary: "bg-brand text-brand-foreground hover:opacity-95",

    secondary: "border border-border bg-bg text-text hover:bg-surface-hover",

    ghost: "bg-transparent text-text hover:bg-surface-hover",
  };

  const sizes: Record<ButtonSize, string> = {
    sm: "px-2.5 py-1.5 text-sm",
    md: "px-3 py-1.5 text-sm",
  };

  const mergedClassName = [base, variants[variant], sizes[size], className]
    .filter(Boolean)
    .join(" ");

  // asChild: style + props are applied to the single child element (e.g. <Link/>)
  if (asChild) {
    if (!isValidElement(children)) {
      throw new Error(
        "Button with asChild expects a single React element child.",
      );
    }

    const child = children as React.ReactElement<any>;
    const childClassName = [mergedClassName, child.props?.className]
      .filter(Boolean)
      .join(" ");

    return cloneElement(child, {
      ...props,
      className: childClassName,
    });
  }

  return (
    <button className={mergedClassName} {...props}>
      {children}
    </button>
  );
}
